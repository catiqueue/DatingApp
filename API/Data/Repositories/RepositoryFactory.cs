using API.Services.Abstractions.Repositories;

namespace API.Data.Repositories;

public sealed class RepositoryFactory(IServiceProvider services) : IRepositoryFactory {
  public T GetRepository<T>() where T : class, IRepository => services.GetRequiredService<T>();
}

public interface IRepositoryFactory {
  public T GetRepository<T>() where T : class, IRepository;
}
