using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

using API.Data;
using API.Data.DTOs;
using API.Data.Repositories;
using API.Data.Requests;
using API.Entities;
using API.Services;
using API.Services.Abstractions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public sealed class AccountController(IUserRepository users, ITokenService tokenSvc) : ApiControllerBase {
  [HttpPost("register")]
  public async Task<ActionResult<AuthenticatedUser>> Handle(RegisterRequest request) {
    if (await users.UserExistsAsync(request.Username)) return BadRequest("The username is already taken.");
    return Ok("Under construction.");
    
    // Postponed for now
    // using var encoder = new HMACSHA512();
    //
    // var newUser = new AppUser {
    //   Username = request.Username,
    //   // So I'm doing this thing where I'm storing the hashes as strings, which is not ideal,
    //   // but I'll stick with my bad decision for now.
    //   Base64PasswordHash = Convert.ToBase64String(encoder.ComputeHash(Encoding.UTF8.GetBytes(request.Password))),
    //   Base64PasswordSalt = Convert.ToBase64String(encoder.Key)
    // };
    //
    // db.Users.Add(newUser);
    // await db.SaveChangesAsync();
    //
    // return Ok(new AuthenticatedUser(newUser.Username, tokenSvc.CreateToken(newUser)));
  }
  
  [HttpPost("login")]
  public async Task<ActionResult<AuthenticatedUser>> Handle(LoginRequest request) {
    var user = await users.GetDbUserAsync(request.Username);
    if (user == null) return Unauthorized("The username was not found.");
    
    using var encoder = new HMACSHA512(Convert.FromBase64String(user.Base64PasswordSalt));

    //See the comment in the Register endpoint about these conversions.
    var passwordHash = Convert.ToBase64String(encoder.ComputeHash(Encoding.UTF8.GetBytes(request.Password)));
    if (passwordHash != user.Base64PasswordHash) return Unauthorized("The password was incorrect.");
    
    return Ok(new AuthenticatedUser(user.Username, tokenSvc.CreateToken(user), user.Photos.FirstOrDefault(x => x.IsMain)?.Url));
  }
}
