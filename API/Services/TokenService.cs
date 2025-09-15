using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using API.Entities;
using API.Extensions.Configuration;
using API.Interfaces;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config, UserManager<User> userManager) : ITokenService {
  public async Task<string> CreateToken(User user) {
    if(user.UserName is null) throw new ArgumentException("The username is null.", nameof(user));
    var tokenKey = config.GetJwtSymmetricalKey();
    
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
    
    Claim[] claims = [
      // ReSharper disable ArrangeObjectCreationWhenTypeNotEvident // Stupid machine
      new(ClaimTypes.Name, user.UserName),
      new(ClaimTypes.NameIdentifier, user.Id.ToString()),
      ..(await userManager.GetRolesAsync(user)).Select(role => new Claim(ClaimTypes.Role, role))
      // ReSharper restore ArrangeObjectCreationWhenTypeNotEvident
    ];
    
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

    var descriptor = new SecurityTokenDescriptor {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.UtcNow.AddDays(7),
      SigningCredentials = credentials
    };
    
    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(descriptor);
    return tokenHandler.WriteToken(token);
  }
}
