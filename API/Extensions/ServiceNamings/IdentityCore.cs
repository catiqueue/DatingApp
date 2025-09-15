using API.Data;
using API.Entities;

using Microsoft.AspNetCore.Identity;

namespace API.Extensions;

public static partial class ServiceNamings {
  public static IServiceCollection AddIdentityCore(this IServiceCollection services) => services
    .AddIdentityCore<User>(options => {
      options.Password.RequireNonAlphanumeric = false;
      options.Password.RequiredLength = 8;
    })
    .AddRoles<Role>()
    .AddRoleManager<RoleManager<Role>>()
    .AddEntityFrameworkStores<ApiDbContext>().Services;
}
