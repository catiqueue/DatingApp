namespace API.Interfaces.Repositories;

public interface IRepositoryFactory {
  public T GetRepository<T>() where T : IRepository;
}
