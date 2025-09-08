namespace API.Extensions;

public static class ServiceCollectionExtensions {
  public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    => services.AddSqliteDbContext(configuration)
               .AddCors()
               .AddControllers().Services
               .AddServices(configuration)
               .AddRepositories()
               .AddAutoMapper(typeof(Program).Assembly)
               .AddSignalR().Services
               .AddPresenceTracker()
               .AddUserActivityTracker();

  public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    => services.AddIdentityCore()
               .AddPolicies()
               .AddDefaultJwtAuthentication(configuration);
}
