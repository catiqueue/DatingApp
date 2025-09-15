using API.Interfaces.Repositories;
using API.Repositories;

namespace API.Extensions;

public static partial class ServiceNamings {
  public static IServiceCollection AddRepositories(this IServiceCollection services)
    => services.AddScoped<IUserRepository, UserRepository>()
               .AddScoped<ILikeRepository, LikeRepository>()
               .AddScoped<IMessageRepository, MessageRepository>()
               .AddScoped<IPhotoRepository, PhotoRepository>();
}
