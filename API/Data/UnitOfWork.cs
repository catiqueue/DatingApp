using API.Interfaces;
using API.Interfaces.Repositories;

namespace API.Data;

public sealed class UnitOfWork(ApiDbContext db, IRepositoryFactory repositories) : IUnitOfWork {
  public IUserRepository Users { get; } = repositories.GetRepository<IUserRepository>();
  public IMessageRepository Messages { get; } = repositories.GetRepository<IMessageRepository>();
  public ILikeRepository Likes { get; } = repositories.GetRepository<ILikeRepository>();
  public IPhotoRepository Photos { get; } = repositories.GetRepository<IPhotoRepository>();
  public bool HasChanges => db.ChangeTracker.HasChanges();
  public async Task<bool> TrySaveAllAsync() => await db.SaveChangesAsync() > 0;
}
