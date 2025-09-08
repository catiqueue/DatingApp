using API.Data.Requests;
using API.Data.Responses;
using API.Entities;
using API.Services.Abstractions;

using AutoMapper;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public sealed class AccountController(UserManager<DbUser> userManager, ITokenService tokenSvc, IMapper mapper) : ApiControllerBase {
  [HttpPost("register")]
  public async Task<ActionResult<AuthenticatedUser>> Handle(RegisterRequest request) {
    if (await UserExistsAsync(request.Username)) 
      return BadRequest("The username is already taken.");
    
    var user = mapper.Map<DbUser>(request);

    return (await userManager.CreateAsync(user, request.Password)).Succeeded
      ? Ok(AuthenticatedUser.FromDbUser(user, await tokenSvc.CreateToken(user)))
      : BadRequest("Failed to register the user.");
  }
  
  [HttpPost("login")]
  public async Task<ActionResult<AuthenticatedUser>> Handle(LoginRequest request) {
    if (await userManager.Users.Include(user => user.Photos)
          .FirstOrDefaultAsync(user => user.NormalizedUserName == userManager.NormalizeName(request.Username))
        is not { } user)
      return Unauthorized("The username was not found.");

    return !await userManager.CheckPasswordAsync(user, request.Password)
      ? Unauthorized("The password was incorrect.")
      : Ok(AuthenticatedUser.FromDbUser(user, await tokenSvc.CreateToken(user)));
    // ? Ok(AuthenticatedUser.FromDbUser(user, tokenSvc.CreateToken(user)))
    // : Unauthorized("The password was incorrect.");
  }
  
  private async Task<bool> UserExistsAsync(string username) => await userManager.Users.AnyAsync(u => u.NormalizedUserName == userManager.NormalizeName(username));
}
