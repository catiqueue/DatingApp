using API.Data.DTOs;
using API.Data.Responses;
using API.Entities;
using API.Helpers;

namespace API.Services.Abstractions;

// TODO: string overloads don't deserve to live
public interface IUserRepository {
  void Update(DbUser user);
  
  Task<bool> TrySaveAllAsync();

  Task<int> CountAsync(UserFilter filter);
  
  Task UpdateLastActiveAsync(uint id);
  Task UpdateLastActiveAsync(string username);
  
  Task<IEnumerable<DbUser>> GetDbUsersAsync(Page page, UserFilter filter, UserSortOrder? sortOrder);
  
  
  Task<DbUser?> GetDbUserAsync(uint id);
  Task<DbUser?> GetDbUserAsync(string username);
  
  Task AddDbUserAsync(DbUser user);
  
  Task<bool> UserExistsAsync(string username);
  Task<bool> UserExistsAsync(uint id);
  
  Task<IEnumerable<SimpleUser>> GetSimpleUsersAsync(Page page, UserFilter filter, UserSortOrder? sortOrder);
  
  Task<SimpleUser?> GetSimpleUserAsync(uint id);
  Task<SimpleUser?> GetSimpleUserAsync(string username);
}
