using API.Data.DTOs;
using API.Data.Requests;
using API.Data.Responses;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Services.Abstractions;
using API.Services.Abstractions.PhotoService;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository users, IMapper mapper, IPhotoService photoService) : ApiControllerBase {
  
  [HttpGet]
  public async Task<ActionResult<PaginatedResponse<SimpleUser>>> GetUsers([FromQuery] GetUsersRequest request) =>
    PaginationInfo.TryCreate(request.ToPage(), await users.CountAsync(request.ToFilter(User.GetUsername())), out var paginationInfo)
      ? Ok(PaginatedResponse<SimpleUser>.FromPaginationInfo(
          paginationInfo,
          await users.GetSimpleUsersAsync(paginationInfo.Page, request.ToFilter(User.GetUsername()), request.OrderBy)))
      // don't shout
      : request.PageNumber == 1
        ? Ok(PaginatedResponse<SimpleUser>.Empty(request.PageSize))
        : BadRequest($"Page {request.PageNumber} does not exist.");

  [HttpGet("{id:int}")] 
  public async Task<ActionResult<SimpleUser>> GetUser(uint id) 
    => await users.GetDbUserAsync(id) is null ? NotFound() : Ok(await users.GetSimpleUserAsync(id));
  
  [HttpGet("{username}")] 
  public async Task<ActionResult<SimpleUser>> GetUser(string username) 
    => await users.GetSimpleUserAsync(username) is { } simpleUser ? Ok(simpleUser) : NotFound();

  [HttpPut]
  public async Task<ActionResult> UpdateUser(UpdateUserRequest request) {
    if (await users.GetDbUserAsync(User.GetUsername()) is not { } user) 
      return BadRequest("Could not find you in the database. How did you do that?");

    mapper.Map(request, user);

    return await users.TrySaveAllAsync() ? NoContent() : BadRequest("Failed to update the user.");
  }
  
  [HttpPost("add-photo")]
  public async Task<ActionResult<SimplePhoto>> AddPhoto(IFormFile file) {
    if (await users.GetDbUserAsync(User.GetUsername()) is not { } user) 
      return BadRequest("Could not find you in the database. How did you do that?");
    
    var result = await photoService.UploadPhotoAsync(file);
    if (result.IsFailure) return BadRequest(result.Error.Message);

    var photo = new DbPhoto {
      Url = result.Value.SecureUrl,
      PublicId = result.Value.PublicId,
    };
    
    if(user.Photos.Count == 0) photo.IsMain = true;
    
    user.Photos.Add(photo);

    return await users.TrySaveAllAsync() 
      ? CreatedAtAction(nameof(GetUser), new { id = user.Id }, mapper.Map<SimplePhoto>(photo))
      : BadRequest("Failed to add the photo.");
  }
  
  [HttpPut("set-main-photo/{photoId:int}")]
  public async Task<ActionResult> SetMainPhoto(uint photoId) {
    if (await users.GetDbUserAsync(User.GetUsername()) is not { } user) 
      return BadRequest("Could not find you in the database. How did you do that?");
    
    if (user.Photos.FirstOrDefault(p => p.Id == photoId) is not { } photo || photo.IsMain) 
      return BadRequest("Failed to set the main photo.");
    
    if (user.Photos.FirstOrDefault(p => p.IsMain) is { } previousMain) 
      previousMain.IsMain = false;
    
    photo.IsMain = true;
    
    return await users.TrySaveAllAsync() ? NoContent() : BadRequest("Failed to set the main photo.");
  }
  
  [HttpDelete("delete-photo/{photoId:int}")]
  public async Task<ActionResult> DeletePhoto(uint photoId) {
    if (await users.GetDbUserAsync(User.GetUsername()) is not { } user) 
      return BadRequest("Could not find you in the database. How did you do that?");
    
    if (user.Photos.FirstOrDefault(p => p.Id == photoId) is not { } photo || photo.IsMain) 
      return BadRequest("Failed to delete the photo.");
    
    if(photo.PublicId is not null && await photoService.DeletePhotoAsync(photo.PublicId) is { } error) 
      return BadRequest(error.Message);
    
    user.Photos.Remove(photo);
    return await users.TrySaveAllAsync() ? Ok() : BadRequest("Failed to delete the photo.");
  }
}
