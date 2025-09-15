using API.Interfaces.Repositories;

namespace API.Interfaces;

public interface IUnitOfWork {
  IUserRepository Users { get; }
  IMessageRepository Messages { get; }
  ILikeRepository Likes { get; }
  IPhotoRepository Photos { get; }
  bool HasChanges { get; }
  
  Task<bool> TrySaveAllAsync();
  
}
