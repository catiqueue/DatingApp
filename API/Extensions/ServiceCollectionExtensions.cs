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

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class ServiceCollectionExtensions {
  public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    => services.AddSqliteDbContext(configuration)
               .AddCors()
               .AddControllers().Services
               .AddServices(configuration)
               .AddRepositories()
               .AddAutoMapper(typeof(Program).Assembly)
               .AddUserActivityTracker();
  
  private static IServiceCollection AddSqliteDbContext(this IServiceCollection services, IConfiguration configuration)
    => services.AddDbContext<DataContext>(options =>
        options.UseSqlite(configuration.GetSqliteConnectionString()));
  
  private static IServiceCollection AddRepositories(this IServiceCollection services)
    => services.AddScoped<IUserRepository, UserRepository>()
               .AddScoped<ILikesRepository, LikeRepository>()
               .AddScoped<IMessagesRepository, MessageRepository>();

  private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    => services.AddScoped<ITokenService, TokenService>()
               .AddCloudinaryPhotoService(configuration);
  
  private static IServiceCollection AddCloudinaryPhotoService(this IServiceCollection services, IConfiguration configuration) 
    => services.Configure<CloudinaryOptions>(configuration.GetCloudinarySection())
               .AddScoped<IPhotoService, CloudinaryPhotoService>();

  public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    => services.AddIdentityCore<DbUser>(options => {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;
      })
        .AddRoles<DbRole>()
        .AddRoleManager<RoleManager<DbRole>>()
        .AddEntityFrameworkStores<DataContext>().Services
      .AddDefaultJwtAuthentication(configuration)
      .AddAuthorizationBuilder()
        .AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"))
        .AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator")).Services;
  
  
  private static IServiceCollection AddDefaultJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    => services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options => {
        var tokenKey = configuration.GetJwtSymmetricalKey();
        options.TokenValidationParameters = new TokenValidationParameters {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
          ValidateIssuer = false,
          ValidateAudience = false
        };
      }).Services;  
  
  private static IServiceCollection AddUserActivityTracker(this IServiceCollection services)
    => services.AddScoped<UserActivityTracker>();
}
