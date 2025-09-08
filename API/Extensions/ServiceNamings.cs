using System.Text;

using API.Data;
using API.Data.Repositories;
using API.Entities;
using API.Extensions.Configuration;
using API.Helpers;
using API.Services;
using API.Services.Abstractions;
using API.Services.Abstractions.PhotoService;
using API.Services.Abstractions.Repositories;
using API.SignalR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class ServiceNamings {
  public static IServiceCollection AddSqliteDbContext(this IServiceCollection services, IConfiguration configuration)
    => services.AddDbContext<DataContext>(options =>
      options.UseSqlite(configuration.GetSqliteConnectionString()));
  
  
  public static IServiceCollection AddUnitOfWork(this IServiceCollection services) 
    => services.AddScoped<IUnitOfWork, UnitOfWork>();
  
  
  public static IServiceCollection AddRepositoryFactory(this IServiceCollection services) 
    => services.AddScoped<IRepositoryFactory, RepositoryFactory>();
  
  
  public static IServiceCollection AddRepositories(this IServiceCollection services)
    => services.AddScoped<IUserRepository, UserRepository>()
      .AddScoped<ILikesRepository, LikeRepository>()
      .AddScoped<IMessagesRepository, MessageRepository>();

  
  
  public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    => services.AddScoped<ITokenService, TokenService>()
      .AddCloudinaryPhotoService(configuration);
  
  
  
  public static IServiceCollection AddCloudinaryPhotoService(this IServiceCollection services, IConfiguration configuration) 
    => services.Configure<CloudinaryOptions>(configuration.GetCloudinarySection())
      .AddScoped<IPhotoService, CloudinaryPhotoService>();
  
  
  
  public static IServiceCollection AddUserActivityTracker(this IServiceCollection services)
    => services.AddScoped<UserActivityTracker>();
  
  
  
  public static IServiceCollection AddPresenceTracker(this IServiceCollection services)
    => services.AddSingleton<IPresenceTracker, StaticPresenceTracker>();
  
  
  
  public static IServiceCollection AddDefaultJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    => services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options => {
        var tokenKey = configuration.GetJwtSymmetricalKey();
        options.TokenValidationParameters = new TokenValidationParameters {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
          ValidateIssuer = false,
          ValidateAudience = false
        };
        options.Events = new JwtBearerEvents {
          OnMessageReceived = context => {
            var accessToken = context.HttpContext.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs")) {
              context.Token = accessToken;
            }
            return Task.CompletedTask;
          }
        };
      }).Services;

  
  
  public static IServiceCollection AddPolicies(this IServiceCollection services)
    => services.AddAuthorizationBuilder()
      .AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"))
      .AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator")).Services;

  
  
  public static IServiceCollection AddIdentityCore(this IServiceCollection services) => services
    .AddIdentityCore<DbUser>(options => {
      options.Password.RequireNonAlphanumeric = false;
      options.Password.RequiredLength = 8;
    })
    .AddRoles<DbRole>()
    .AddRoleManager<RoleManager<DbRole>>()
    .AddEntityFrameworkStores<DataContext>().Services;
}
