using API.Helpers;

namespace API.Extensions;

public static partial class ServiceNamings {
  public static IServiceCollection AddUserActivityTracker(this IServiceCollection services)
    => services.AddScoped<UserActivityTracker>();
}
