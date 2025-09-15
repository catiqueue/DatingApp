using API.Interfaces.Repositories;

namespace API.Repositories;

public sealed class RepositoryFactory(IServiceProvider services) : IRepositoryFactory {
  public T GetRepository<T>() where T : IRepository => services.GetRequiredService<T>();
}
