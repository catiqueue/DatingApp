using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

using API.Data;
using API.Data.DTOs;
using API.Data.Repositories;
using API.Data.Requests;
using API.Data.Responses;
using API.Entities;
using API.Services;
using API.Services.Abstractions;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public sealed class AccountController(IUserRepository users, ITokenService tokenSvc, IMapper mapper) : ApiControllerBase {
  [HttpPost("register")]
  public async Task<ActionResult<AuthenticatedUser>> Handle(RegisterRequest request) {
    if (await users.UserExistsAsync(request.Username)) return BadRequest("The username is already taken.");
    
    using var encoder = new HMACSHA512();
    var user = mapper.Map<DbUser>(request);

    user.Base64PasswordHash = Convert.ToBase64String(encoder.ComputeHash(Encoding.UTF8.GetBytes(request.Password)));
    user.Base64PasswordSalt = Convert.ToBase64String(encoder.Key);
    
    await users.AddDbUserAsync(user);
    return await users.TrySaveAllAsync()
      ? Ok(AuthenticatedUser.FromDbUser(user, tokenSvc.CreateToken(user)))
      : BadRequest("Failed to register the user.");
  }
  
  [HttpPost("login")]
  public async Task<ActionResult<AuthenticatedUser>> Handle(LoginRequest request) {
    var user = await users.GetDbUserAsync(request.Username);
    if (user == null) return Unauthorized("The username was not found.");
    
    using var encoder = new HMACSHA512(Convert.FromBase64String(user.Base64PasswordSalt));
    
    var base64PasswordHash = Convert.ToBase64String(encoder.ComputeHash(Encoding.UTF8.GetBytes(request.Password)));
    return base64PasswordHash == user.Base64PasswordHash
      ? Ok(AuthenticatedUser.FromDbUser(user, tokenSvc.CreateToken(user)))
      : Unauthorized("The password was incorrect.");
  }
}
