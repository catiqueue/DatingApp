using API.DTO.Requests;
using API.DTO.Responses;
using API.Entities;
using API.Interfaces;

using AutoMapper;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public sealed class AccountController(UserManager<User> userManager, ITokenService tokenSvc, IMapper mapper) : ApiControllerBase {
  [HttpPost("register")]
  public async Task<ActionResult<AuthenticatedUserDto>> Handle(RegisterRequest request) {
    if (await UserExistsAsync(request.Username)) 
      return BadRequest("The username is already taken.");
    
    var user = mapper.Map<User>(request);

    if (!(await userManager.CreateAsync(user, request.Password)).Succeeded)
      BadRequest("Failed to register the user.");
    
    if (!(await userManager.AddToRoleAsync(user, "User")).Succeeded)
      BadRequest("Failed to add the user to the default role.");

    return Ok(AuthenticatedUserDto.FromDbUser(user, await tokenSvc.CreateToken(user)));
  }
  
  [HttpPost("login")]
  public async Task<ActionResult<AuthenticatedUserDto>> Handle(LoginRequest request) {
    if (await userManager.Users.Include(user => user.Photos)
          .FirstOrDefaultAsync(user => user.NormalizedUserName == userManager.NormalizeName(request.Username))
        is not { } user)
      return Unauthorized("The username was not found.");

    return !await userManager.CheckPasswordAsync(user, request.Password)
      ? Unauthorized("The password was incorrect.")
      : Ok(AuthenticatedUserDto.FromDbUser(user, await tokenSvc.CreateToken(user)));
  }
  
  private async Task<bool> UserExistsAsync(string username) => await userManager.Users.AnyAsync(u => u.NormalizedUserName == userManager.NormalizeName(username));
}
