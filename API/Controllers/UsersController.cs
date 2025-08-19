using API.Data.DTOs;
using API.Data.Repositories;
using API.Data.Requests;
using API.Entities;
using API.Extensions;
using API.Services.Abstractions.PhotoService;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository users, IMapper mapper, IPhotoService photoService) : ApiControllerBase {
    
  [HttpGet("all")]
  public async Task<ActionResult<IEnumerable<SimpleUser>>> GetUsers() 
    => Ok(await users.GetSimpleUsersAsync());

  [HttpGet("{id:int}")] 
  public async Task<ActionResult<SimpleUser>> GetUser(uint id) {
    var user = await users.GetDbUserAsync(id);
    if(user == null) return NotFound();
    return Ok(await users.GetSimpleUserAsync(id));
  }
  
  [HttpGet("{username}")] 
  public async Task<ActionResult<SimpleUser>> GetUser(string username) {
    var user = await users.GetDbUserAsync(username);
    return user == null ? NotFound() : Ok(await users.GetSimpleUserAsync(username));
  }

  [HttpPut]
  public async Task<ActionResult> UpdateUser(UpdateUserRequest request) {
    var user = await users.GetDbUserAsync(User.GetUsername());
    if (user is null) return BadRequest("Could not find you in the database. How did you do that?");

    mapper.Map(request, user);

    return await users.TrySaveAllAsync() ? NoContent() : BadRequest("Failed to update the user.");
  }
  
  [HttpPost("add-photo")]
  public async Task<ActionResult<SimplePhoto>> AddPhoto(IFormFile file) {
    var user = await users.GetDbUserAsync(User.GetUsername());
    if (user is null) return BadRequest("Could not find you in the database. How did you do that?");
    
    var result = await photoService.UploadPhotoAsync(file);
    if (!result.IsSuccessful) return BadRequest(result.Error.Message);

    var photo = new DbPhoto {
      Url = result.Value.SecureUrl,
      PublicId = result.Value.PublicId,
    };
    
    user.Photos.Add(photo);

    return await users.TrySaveAllAsync() 
      ? CreatedAtAction(nameof(GetUser), new { id = user.Id }, mapper.Map<SimplePhoto>(photo))
      : BadRequest("Failed to add the photo.");
  }
  
  [HttpPut("set-main-photo/{photoId:int}")]
  public async Task<ActionResult> SetMainPhoto(uint photoId) {
    var user = await users.GetDbUserAsync(User.GetUsername());
    if (user is null) return BadRequest("Could not find you in the database. How did you do that?");

    var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
    if (photo is null || photo.IsMain) return BadRequest("Failed to set the main photo.");

    var previousMain = user.Photos.FirstOrDefault(p => p.IsMain);
    if (previousMain is not null) previousMain.IsMain = false;
    photo.IsMain = true;
    
    return await users.TrySaveAllAsync() ? NoContent() : BadRequest("Failed to set the main photo.");
  }
  
  [HttpDelete("delete-photo/{photoId:int}")]
  public async Task<ActionResult> DeletePhoto(uint photoId) {
    var user = await users.GetDbUserAsync(User.GetUsername());
    if (user is null) return BadRequest("Could not find you in the database. How did you do that?");

    var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
    if (photo is null || photo.IsMain) return BadRequest("Failed to delete the photo.");
    
    if(photo.PublicId is not null 
       && await photoService.DeletePhotoAsync(photo.PublicId) is { HasValue: true } error) return BadRequest(error.Value.Message);
    
    user.Photos.Remove(photo);
    return await users.TrySaveAllAsync() ? Ok() : BadRequest("Failed to delete the photo.");
  }
}
