namespace API.Extensions;

public static partial class ServiceNamings {
  public static IServiceCollection AddPolicies(this IServiceCollection services)
    => services.AddAuthorizationBuilder()
      .AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"))
      .AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator")).Services;
}
