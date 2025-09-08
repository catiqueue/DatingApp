using API.Data.Repositories;
using API.Services.Abstractions;
using API.Services.Abstractions.Repositories;

namespace API.Data;

public sealed class UnitOfWork(DataContext db, IRepositoryFactory repositories) : IUnitOfWork {
  public IUserRepository Users { get; } = repositories.GetRepository<IUserRepository>();
  public IMessagesRepository Messages { get; } = repositories.GetRepository<IMessagesRepository>();
  public ILikesRepository Likes { get; } = repositories.GetRepository<ILikesRepository>();
  public bool HasChanges => db.ChangeTracker.HasChanges();
  public async Task<bool> TrySaveAllAsync() => await db.SaveChangesAsync() > 0;
}
