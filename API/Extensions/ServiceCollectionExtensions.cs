namespace API.Extensions;

public static class ServiceCollectionExtensions {
  public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    => services.AddHttpContextAccessor()
               .AddPostgreSqlDbContext(configuration)
               // .AddSqliteDbContext(configuration)
               .AddCors()
               .AddControllers().Services
               .AddExceptionHandlers()
               .AddServices(configuration)
               .AddRepositories()
               .AddRepositoryFactory()
               .AddUnitOfWork()
               .AddAutoMapper(typeof(Program).Assembly)
               .AddSignalR().Services
               .AddUserActivityTracker();

  public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    => services.AddIdentityCore()
               .AddPolicies()
               .AddDefaultJwtAuthentication(configuration);
}
