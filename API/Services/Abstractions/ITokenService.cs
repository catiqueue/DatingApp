using API.Entities;

namespace API.Services.Abstractions;

public interface ITokenService {
  string CreateToken(DbUser user); 
}
