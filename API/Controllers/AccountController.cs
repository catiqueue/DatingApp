using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

using API.Data;
using API.Data.DTObjects;
using API.Data.Requests;
using API.Entities;
using API.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public sealed class AccountController(DataContext db, ITokenService tokenSvc) : ApiControllerBase {
  [HttpPost("register")]
  public async Task<ActionResult<AuthenticatedUser>> Handle(RegisterRequest request) {
    if (await UserExistsAsync(request.Username)) return BadRequest("The username is already taken.");
    
    using var encoder = new HMACSHA512();

    var newUser = new AppUser {
      Username = request.Username,
      // So I'm doing this thing where I'm storing the hashes as strings, which is not ideal,
      // but I'll stick with my bad decision for now.
      PasswordHash = Convert.ToBase64String(encoder.ComputeHash(Encoding.UTF8.GetBytes(request.Password))),
      PasswordSalt = Convert.ToBase64String(encoder.Key)
    };

    db.Users.Add(newUser);
    await db.SaveChangesAsync();
    
    return Ok(new AuthenticatedUser(newUser.Username, tokenSvc.CreateToken(newUser)));
  }
  
  [HttpPost("login")]
  public async Task<ActionResult<AuthenticatedUser>> Handle(LoginRequest request) {
    var user = await GetUserAsync(request.Username);
    if (user == null) return Unauthorized("The username was not found.");
    
    using var encoder = new HMACSHA512(Convert.FromBase64String(user.PasswordSalt));

    //See the comment in the Register endpoint about these conversions.
    var passwordHash = Convert.ToBase64String(encoder.ComputeHash(Encoding.UTF8.GetBytes(request.Password)));
    if (passwordHash != user.PasswordHash) return Unauthorized("The password was incorrect.");
    
    return Ok(new AuthenticatedUser(user.Username, tokenSvc.CreateToken(user)));
  }
  
  private async Task<AppUser?> GetUserAsync(string username) => await db.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
  [SuppressMessage("Performance", "CA1862:Use the \'StringComparison\' method overloads to perform case-insensitive string comparisons")]
  private async Task<bool> UserExistsAsync(string username) => await db.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower());
}
