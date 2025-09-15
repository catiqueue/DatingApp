using API.DTO.Responses;
using API.Entities;
using API.Helpers;

namespace API.Interfaces.Repositories;

// TODO: string overloads should be removed
public interface IUserRepository : IRepository {
  void Update(User user);

  Task<int> CountAsync(UserFilter filter);
  
  Task ExecuteUpdateLastActiveAsync(int id);
  Task ExecuteUpdateLastActiveAsync(string username);
  
  Task<IEnumerable<User>> GetUsersAsync(Page page, UserFilter filter, UserSortOrder? sortOrder);
  
  
  Task<User?> GetUserAsync(int id);
  Task<User?> GetUserAsync(string username);
  Task<User?> GetUserByPhotoIdAsync(int photoId);
  
  Task AddUserAsync(User user);
  
  Task<bool> UserExistsAsync(string username);
  Task<bool> UserExistsAsync(int id);
  
  Task<IEnumerable<UserDto>> GetUserDtosAsync(Page page, UserFilter filter, UserSortOrder? sortOrder);
  
  Task<UserDto?> GetUserDtoAsync(int id);
  Task<UserDto?> GetUserDtoAsync(string username);
}
