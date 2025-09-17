using API.Data;
using API.Extensions.Configuration;

using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static partial class ServiceNamings {
  public static IServiceCollection AddPostgreSqlDbContext(this IServiceCollection services, IConfiguration configuration)
    => services.AddDbContext<ApiDbContext>(options => options.UseNpgsql(configuration.GetPostgreSqlConnectionString()));
}
