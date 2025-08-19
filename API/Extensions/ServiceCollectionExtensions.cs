using System.Text;

using API.Data;
using API.Data.Repositories;
using API.Extensions.Configuration;
using API.Services;
using API.Services.Abstractions;
using API.Services.Abstractions.PhotoService;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class ServiceCollectionExtensions {
  public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    => services.AddSqliteDbContext(configuration)
      .AddCors()
      .AddControllers().Services
      .AddDefaultJwtTokenService()
      .AddUserRepository()
      .AddAutoMapper(typeof(Program).Assembly)
      .ConfigureCloudinary(configuration)
      .AddCloudinaryPhotoService()
      .AddDefaultJwtAuthentication(configuration);

  private static IServiceCollection AddSqliteDbContext(this IServiceCollection services, IConfiguration configuration)
    => services.AddDbContext<DataContext>(options =>
      options.UseSqlite(configuration.GetSqliteConnectionString()));

  private static IServiceCollection AddDefaultJwtTokenService(this IServiceCollection services)
    => services.AddScoped<ITokenService, TokenService>();
  
  private static IServiceCollection AddUserRepository(this IServiceCollection services)
    => services.AddScoped<IUserRepository, UserRepository>();
  
  private static IServiceCollection ConfigureCloudinary(this IServiceCollection services, IConfiguration configuration) 
    => services.Configure<CloudinaryOptions>(configuration.GetCloudinarySection());
  
  private static IServiceCollection AddCloudinaryPhotoService(this IServiceCollection services) 
    => services.AddScoped<IPhotoService, CloudinaryPhotoService>();

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
}
