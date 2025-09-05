using API.Entities;

namespace API.Services.Abstractions;

public interface ITokenService {
  Task<string> CreateToken(DbUser user); 
}
