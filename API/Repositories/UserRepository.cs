using API.Data;
using API.DTO.Responses;
using API.Entities;
using API.Helpers;
using API.Interfaces.Repositories;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class UserRepository(ApiDbContext db, IMapper mapper) : IUserRepository {
  public void Update(User user) => db.Entry(user).State = EntityState.Modified;
  
  public async Task<int> CountAsync(UserFilter filter) => await ReadonlyUsers.Filter(filter).CountAsync();

  public async Task ExecuteUpdateLastActiveAsync(int id) 
    => await db.Users.Where(user => user.Id == id)
      .ExecuteUpdateAsync(setters => setters.SetProperty(user => user.LastActive, DateTime.UtcNow));
  
  public async Task ExecuteUpdateLastActiveAsync(string username) 
    => await db.Users.Where(u => u.UserName == username)
      .ExecuteUpdateAsync(setters => setters.SetProperty(user => user.LastActive, DateTime.UtcNow));

  public async Task<IEnumerable<User>> GetUsersAsync(Page page, UserFilter filter, UserSortOrder? sortOrder) 
    => await WritableUsers
      .IgnoreQueryFilters()
      .WithPhotos()
      .Filter(filter)
      .Sort(sortOrder)
      .Slice(page)
      .ToListAsync();

  public async Task<User?> GetUserAsync(int id) 
    => await WritableUsers
      .IgnoreQueryFilters()
      .WithPhotos()
      .SingleOrDefaultAsync(user => user.Id == id);
  
  public async Task<User?> GetUserAsync(string username) 
    => await WritableUsers
      .IgnoreQueryFilters()
      .WithPhotos()
      .SingleOrDefaultAsync(u => u.NormalizedUserName == username.Normalize().ToUpperInvariant());
  
  public async Task<User?> GetUserByPhotoIdAsync(int photoId) 
    => await WritableUsers
      .IgnoreQueryFilters()
      .WithPhotos()
      .SingleOrDefaultAsync(u => u.Photos.Any(p => p.Id == photoId));
  
  public async Task AddUserAsync(User user) => await db.Users.AddAsync(user);
  
  public async Task<bool> UserExistsAsync(string username) 
    => await ReadonlyUsers
      .AnyAsync(u => u.NormalizedUserName == username.Normalize().ToUpperInvariant());
  
  public async Task<bool> UserExistsAsync(int id) 
    => await ReadonlyUsers
      .AnyAsync(user => user.Id == id);
  
  public async Task<IEnumerable<UserDto>> GetUserDtosAsync(Page page, UserFilter filter, UserSortOrder? sortOrder)
    => await ReadonlyUsers
      .WithPhotos()
      .Filter(filter)
      .Sort(sortOrder)
      .Slice(page)
      .ProjectTo<UserDto>(mapper.ConfigurationProvider)
      .ToListAsync();

  public async Task<UserDto?> GetUserDtoAsync(int id) 
    => await ReadonlyUsers
      .WithPhotos()
      .Where(u => u.Id == id)
      .ProjectTo<UserDto>(mapper.ConfigurationProvider)
      .SingleOrDefaultAsync();

  public async Task<UserDto?> GetUserDtoAsync(string username)
    => await ReadonlyUsers
      .WithPhotos()
      .Where(u => u.NormalizedUserName == username.Normalize().ToUpperInvariant())
      .ProjectTo<UserDto>(mapper.ConfigurationProvider)
      .SingleOrDefaultAsync();

  private IQueryable<User> ReadonlyUsers => db.Users.AsNoTracking();
  private IQueryable<User> WritableUsers => db.Users;
}

file static class UserExtensions {
  public static IQueryable<User> WithPhotos(this IQueryable<User> users) 
    => users.Include(u => u.Photos);
}
