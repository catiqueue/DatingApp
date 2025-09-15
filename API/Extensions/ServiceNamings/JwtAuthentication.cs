using System.Text;

using API.Extensions.Configuration;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static partial class ServiceNamings {
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
}
