using API.DTO.Requests;
using API.DTO.Responses;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using API.Interfaces.PhotoService;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
[Authorize(Policy = "RequireAdminRole")]
public class AdminController(UserManager<User> userManager, IUnitOfWork work, IPhotoService photoService) : ApiControllerBase {
  [HttpGet("users-with-roles")]
  public async Task<ActionResult> GetUsersWithRoles()
    => Ok(await userManager.Users
      .Sort(UserSortOrder.Username)
      .Select(u => new {
        u.Id,
        u.UserName,
        Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
      }).ToListAsync());
  
  
  [HttpPost("edit-roles/{username}")]
  public async Task<ActionResult> EditRoles(string username, [FromBody] IEnumerable<string> roles) {
    if(roles.ToArray() is not { Length: > 0 } selectedRoles) 
      return BadRequest("You must select at least one role.");
    if(await userManager.FindByNameAsync(username) is not { } user) 
      return BadRequest("This user does not exist.");
    
    var userRoles = await userManager.GetRolesAsync(user);

    return (await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles))).Succeeded
        && (await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles))).Succeeded 
      ? Ok(await userManager.GetRolesAsync(user)) 
      : BadRequest("Failed to update the roles.");
  }

  [Authorize(Policy = "ModeratePhotoRole")]
  [HttpGet("photos-to-moderate")]
  public async Task<ActionResult<PaginatedResponseDto<UnapprovedPhotoDto>>> Handle([FromQuery] GetPhotosForModerationRequest request)
    => PaginationInfo.TryCreate(request.ToPage(), await work.Photos.CountAsync(PhotoFilter.FromStatus(PhotoApprovalStatus.Pending)), out var paginationInfo)
      ? Ok(PaginatedResponseDto<UnapprovedPhotoDto>.FromPaginationInfo(
        paginationInfo,
        await work.Photos.GetUnapprovedPhotosAsync(request.ToPage())))
      : request.PageNumber == 1
        ? Ok(PaginatedResponseDto<UnapprovedPhotoDto>.Empty(request.PageSize))
        : BadRequest($"Page {request.PageNumber} does not exist.");

  [Authorize(Policy = "ModeratePhotoRole")]
  [HttpPost("approve-photo/{photoId:int}")]
  public async Task<ActionResult> ApprovePhoto(int photoId) {
    // I hate this for some reason
    if(await work.Users.GetUserByPhotoIdAsync(photoId) is not { } user)
      return BadRequest("The photo does not belong to any user.");
    
    if (!user.Photos.Any(p => p.IsMain))
      user.Photos.First(p => p.Id == photoId).IsMain = true;

    return await work.Photos.TryApprovePhotoAsync(photoId) 
      && await work.TrySaveAllAsync()
        ? Ok()
        : BadRequest("Failed to approve the photo.");
  }

  [Authorize(Policy = "ModeratePhotoRole")]
  [HttpPost("reject-photo/{photoId:int}")]
  public async Task<ActionResult> RejectPhoto(int photoId) {
    if(await work.Photos.GetPhotoAsync(photoId) is not { } photo)
      return BadRequest("Can't find the photo.");

    work.Photos.DeletePhoto(photo);
    if (!await work.TrySaveAllAsync()) 
      return BadRequest("Failed to reject the photo.");
    
    if (photo.PublicId is not null && await photoService.DeletePhotoAsync(photo.PublicId) is { } error)
      // What happens if the photo service is down for a couple of hours?
      // Why restrict users from deleting their photos if my application is working fine?
      // I really should just store the PublicIds somewhere and delete those from a background service or something.
      throw new ApplicationException($"Failed to delete the photo from a third-party service: {error.Message}");
    
    return Ok();
  }
}
