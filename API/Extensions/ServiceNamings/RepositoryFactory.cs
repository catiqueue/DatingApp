using API.Interfaces.Repositories;
using API.Repositories;

namespace API.Extensions;

public static partial class ServiceNamings {
  public static IServiceCollection AddRepositoryFactory(this IServiceCollection services) 
    => services.AddScoped<IRepositoryFactory, RepositoryFactory>();
}
