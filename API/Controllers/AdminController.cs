using System.ComponentModel.DataAnnotations;

using API.Entities;
using API.Helpers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AdminController(UserManager<DbUser> userManager) : ApiControllerBase {

  [Authorize(Policy = "RequireAdminRole")]
  [HttpGet("users-with-roles")]
  public async Task<ActionResult> GetUsersWithRoles()
    => Ok(await userManager.Users
      .Sort(UserSortOrder.Username)
      .Select(u => new {
        u.Id,
        u.UserName,
        Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
      }).ToListAsync());
  
  [Authorize(Policy = "RequireAdminRole")]
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
  public async Task<ActionResult> GetPhotosForModeration() => Ok("Photo moderator secret");
}
