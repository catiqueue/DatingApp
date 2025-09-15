using API.Interfaces;
using API.Services;

namespace API.Extensions;

public static partial class ServiceNamings {
  public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    => services.AddScoped<ITokenService, TokenService>()
      .AddCloudinaryPhotoService(configuration)
      .AddSingleton<IPresenceTrackerService, StaticPresenceTrackerService>();
}
