using API.Data.Responses;
using API.Entities;
using API.Helpers;
using API.Services.Abstractions.Repositories;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class UserRepository(DataContext db, IMapper mapper) : IUserRepository {
  public void Update(DbUser user) => db.Entry(user).State = EntityState.Modified;
  
  public async Task<int> CountAsync(UserFilter filter) => await DbUsers(tracking: false).Filter(filter).CountAsync();

  public async Task UpdateLastActiveAsync(uint id) 
    => await db.Users.Where(user => user.Id == id)
      .ExecuteUpdateAsync(setters => setters.SetProperty(user => user.LastActive, DateTime.UtcNow));
  
  public async Task UpdateLastActiveAsync(string username) 
    => await db.Users.Where(u => u.UserName == username)
      .ExecuteUpdateAsync(setters => setters.SetProperty(user => user.LastActive, DateTime.UtcNow));

  public async Task<IEnumerable<DbUser>> GetDbUsersAsync(Page page, UserFilter filter, UserSortOrder? sortOrder) 
    => await DbUsers()
      .Filter(filter)
      .Sort(sortOrder)
      .Slice(page)
      .ToListAsync();

  public async Task<DbUser?> GetDbUserAsync(uint id) 
    => await DbUsers().SingleOrDefaultAsync(user => user.Id == id);
  
  public async Task<DbUser?> GetDbUserAsync(string username) 
    => await DbUsers().SingleOrDefaultAsync(u => u.NormalizedUserName == username.Normalize().ToUpperInvariant());
  
  public async Task AddDbUserAsync(DbUser user) => await db.Users.AddAsync(user);
  
  public async Task<bool> UserExistsAsync(string username) 
    => await DbUsers(tracking: false)
      .AnyAsync(u => u.NormalizedUserName == username.Normalize().ToUpperInvariant());
  
  public async Task<bool> UserExistsAsync(uint id) 
    => await DbUsers(tracking: false)
      .AnyAsync(user => user.Id == id);
  
  public async Task<IEnumerable<SimpleUser>> GetSimpleUsersAsync(Page page, UserFilter filter, UserSortOrder? sortOrder)
    => await DbUsers(tracking: false)
      .Filter(filter)
      .Sort(sortOrder)
      .Slice(page)
      .ProjectTo<SimpleUser>(mapper.ConfigurationProvider)
      .ToListAsync();

  public async Task<SimpleUser?> GetSimpleUserAsync(uint id) 
    => await DbUsers(tracking: false)
      .Where(u => u.Id == id)
      .ProjectTo<SimpleUser>(mapper.ConfigurationProvider)
      .SingleOrDefaultAsync();

  public async Task<SimpleUser?> GetSimpleUserAsync(string username)
    => await DbUsers(tracking: false)
      .Where(u => u.NormalizedUserName == username.Normalize().ToUpperInvariant())
      .ProjectTo<SimpleUser>(mapper.ConfigurationProvider)
      .SingleOrDefaultAsync();
  
  private IQueryable<DbUser> DbUsers(bool tracking = true) => tracking ? db.Users.Include(u => u.Photos) : db.Users.AsNoTracking().Include(u => u.Photos);
}
