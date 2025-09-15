using API.DTO.Requests;
using API.DTO.Responses;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using API.Interfaces.PhotoService;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController(IUnitOfWork work, IMapper mapper, IPhotoService photoService) : ApiControllerBase {
  
  [HttpGet]
  public async Task<ActionResult<PaginatedResponseDto<UserDto>>> GetUsers([FromQuery] GetUsersRequest request) =>
    PaginationInfo.TryCreate(request.ToPage(), await work.Users.CountAsync(request.ToFilter(User.GetUsername())), out var paginationInfo)
      ? Ok(PaginatedResponseDto<UserDto>.FromPaginationInfo(
          paginationInfo,
          await work.Users.GetUserDtosAsync(paginationInfo.Page, request.ToFilter(User.GetUsername()), request.OrderBy)))
      : request.PageNumber == 1
        ? Ok(PaginatedResponseDto<UserDto>.Empty(request.PageSize))
        : BadRequest($"Page {request.PageNumber} does not exist.");

  [HttpGet("{id:int}")] 
  public async Task<ActionResult<UserDto>> GetUser(int id) 
    => !await work.Users.UserExistsAsync(id) 
      ? NotFound() 
      : Ok(await work.Users.GetUserDtoAsync(id));

  [HttpGet("{username}")]
  public async Task<ActionResult<UserDto>> GetUser(string username) 
    => await work.Users.GetUserDtoAsync(username) is not { } requestingUser 
      ? NotFound() 
      : Ok(requestingUser);

  [HttpPut]
  public async Task<ActionResult> UpdateUser(UpdateUserRequest request) {
    if (await work.Users.GetUserAsync(User.GetUsername()) is not { } user) 
      return BadRequest("Could not find you in the database. How did you do that?");

    mapper.Map(request, user);

    return await work.TrySaveAllAsync() ? NoContent() : BadRequest("Failed to update the user.");
  }
  
  [HttpPost("add-photo")]
  public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file) {
    if (await work.Users.GetUserAsync(User.GetUsername()) is not { } user) 
      return BadRequest("Could not find you in the database. How did you do that?");
    
    var result = await photoService.UploadPhotoAsync(file);
    if (result.IsFailure) return BadRequest(result.Error.Message);

    var photo = new Photo {
      Url = result.Value.SecureUrl,
      PublicId = result.Value.PublicId,
    };
    
    user.Photos.Add(photo);

    return await work.TrySaveAllAsync() 
      ? CreatedAtAction(nameof(GetUser), new { id = user.Id }, mapper.Map<PhotoDto>(photo))
      : BadRequest("Failed to add the photo.");
  }
  
  [HttpPut("set-main-photo/{photoId:int}")]
  public async Task<ActionResult> SetMainPhoto(int photoId) {
    if (await work.Users.GetUserAsync(User.GetUsername()) is not { } user) 
      return BadRequest("Could not find you in the database. How did you do that?");
    
    if (user.Photos.FirstOrDefault(p => p.Id == photoId) is not { } photo || photo.IsMain || !photo.IsApproved) 
      return BadRequest("Failed to set the main photo.");
    
    if (user.Photos.FirstOrDefault(p => p.IsMain) is { } previousMain) 
      previousMain.IsMain = false;
    
    photo.IsMain = true;
    
    return await work.TrySaveAllAsync() ? NoContent() : BadRequest("Failed to set the main photo.");
  }
  
  [HttpDelete("delete-photo/{photoId:int}")]
  public async Task<ActionResult> DeletePhoto(int photoId) {
    if (await work.Users.GetUserAsync(User.GetUsername()) is not { } user) 
      return BadRequest("Could not find you in the database. How did you do that?");
    
    if (user.Photos.FirstOrDefault(p => p.Id == photoId) is not { } photo || photo.IsMain) 
      return BadRequest("Failed to delete the photo.");
    
    user.Photos.Remove(photo);
    if(!await work.TrySaveAllAsync())
      return BadRequest("Failed to delete the photo.");
    
    if(photo.PublicId is not null && await photoService.DeletePhotoAsync(photo.PublicId) is { } error) 
      throw new ApplicationException($"Failed to delete the photo from a third-party service: {error.Message}");

    return Ok();
  }
}
