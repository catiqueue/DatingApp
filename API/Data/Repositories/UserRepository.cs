using API.Data.DTOs;
using API.Entities;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public interface IUserRepository {
  void Update(DbUser user);
  Task<bool> TrySaveAllAsync();
  Task<IEnumerable<DbUser>> GetDbUsersAsync();
  Task<DbUser?> GetDbUserAsync(uint id);
  Task<DbUser?> GetDbUserAsync(string username);

  Task<IEnumerable<SimpleUser>> GetSimpleUsersAsync();
  Task<SimpleUser?> GetSimpleUserAsync(uint id);
  Task<SimpleUser?> GetSimpleUserAsync(string username);
}

public class UserRepository(DataContext db, IMapper mapper) : IUserRepository {
  public void Update(DbUser user) => db.Entry(user).State = EntityState.Modified;

  public async Task<bool> TrySaveAllAsync() => await db.SaveChangesAsync() > 0;

  public async Task<IEnumerable<DbUser>> GetDbUsersAsync() 
    => await db.Users.Include(u => u.Photos).ToListAsync();
  
  public async Task<DbUser?> GetDbUserAsync(uint id) 
    => await db.Users.Include(u => u.Photos).SingleOrDefaultAsync(user => user.Id == id);
  
  public async Task<DbUser?> GetDbUserAsync(string username) 
    => await db.Users.Include(u => u.Photos).SingleOrDefaultAsync(user => user.Username == username.ToLower());

  public async Task<IEnumerable<SimpleUser>> GetSimpleUsersAsync()
    => await db.Users.ProjectTo<SimpleUser>(mapper.ConfigurationProvider).ToListAsync();

  public async Task<SimpleUser?> GetSimpleUserAsync(uint id) 
    => await db.Users.Where(u => u.Id == id)
      .ProjectTo<SimpleUser>(mapper.ConfigurationProvider)
      .SingleOrDefaultAsync();

  public async Task<SimpleUser?> GetSimpleUserAsync(string username)
    => await db.Users.Where(u => u.Username == username.ToLower())
      .ProjectTo<SimpleUser>(mapper.ConfigurationProvider)
      .SingleOrDefaultAsync();
}
