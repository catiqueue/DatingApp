using API.Extensions.Configuration;
using API.Interfaces.PhotoService;
using API.Services;

namespace API.Extensions;

public static partial class ServiceNamings {
  public static IServiceCollection AddCloudinaryPhotoService(this IServiceCollection services, IConfiguration configuration) 
    => services.Configure<CloudinaryOptions>(configuration.GetCloudinarySection())
      .AddScoped<IPhotoService, CloudinaryPhotoService>();
}
