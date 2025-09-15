using API.Data;
using API.Interfaces;

namespace API.Extensions;

public static partial class ServiceNamings {
  public static IServiceCollection AddUnitOfWork(this IServiceCollection services) 
    => services.AddScoped<IUnitOfWork, UnitOfWork>();
}
