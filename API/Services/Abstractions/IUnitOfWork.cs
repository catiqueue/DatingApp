using API.Services.Abstractions.Repositories;

namespace API.Services.Abstractions;

public interface IUnitOfWork {
  IUserRepository Users { get; }
  IMessagesRepository Messages { get; }
  ILikesRepository Likes { get; }
  bool HasChanges { get; }
  
  Task<bool> TrySaveAllAsync();
  
}
