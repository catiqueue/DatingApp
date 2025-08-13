using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using API.Entities;
using API.Extensions.Configuration;

using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public interface ITokenService {
  string CreateToken(DbUser user); 
}

public class TokenService(IConfiguration config) : ITokenService {
  private const string TokenKeySelector = "TokenKey";
  public string CreateToken(DbUser user) {
    var tokenKey = config.GetJwtSymmetricalKey();
    
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
    
    Claim[] claims = [
      // ReSharper disable once ArrangeObjectCreationWhenTypeNotEvident // Stupid machine
      new (JwtRegisteredClaimNames.Nickname, user.Username)
    ];
    
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

    var descriptor = new SecurityTokenDescriptor {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.UtcNow.AddDays(7),
      SigningCredentials = credentials
    };

    // Shut up
    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(descriptor);
    return tokenHandler.WriteToken(token);
  }
}
