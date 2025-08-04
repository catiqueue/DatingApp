using System.Text;

using API.Data;
using API.Extensions.Configuration;
using API.Services;

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
      .AddDefaultJwtAuthentication(configuration);
  
  public static IServiceCollection AddSqliteDbContext(this IServiceCollection services, IConfiguration configuration)
    => services.AddDbContext<DataContext>(options =>
      options.UseSqlite(configuration.GetSqliteConnectionString()));

  public static IServiceCollection AddDefaultJwtTokenService(this IServiceCollection services)
    => services.AddScoped<ITokenService, TokenService>();

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
       }).Services;
}
