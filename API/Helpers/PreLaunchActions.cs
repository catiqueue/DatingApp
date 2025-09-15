using API.Data;
using API.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers;

public static class PreLaunchActions {
  public static async Task ClearConnections(IServiceScope scope)
    => await scope.ServiceProvider.GetRequiredService<ApiDbContext>().Connections.ExecuteDeleteAsync();
  
  public static async Task ApplyMigrations(IServiceScope scope) 
    => await scope.ServiceProvider.GetRequiredService<ApiDbContext>().Database.MigrateAsync();
  
  public static async Task SeedUsers(IServiceScope scope) 
    => await Seed.SeedUsers(scope.ServiceProvider.GetRequiredService<UserManager<User>>(), 
                      scope.ServiceProvider.GetRequiredService<RoleManager<Role>>());
  
  public static async Task ExecuteAction(this WebApplication app, Func<IServiceScope, Task> action) {
    var scope = app.Services.CreateScope();
    try {
      await action(scope);
    } catch (Exception ex) {
      var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
      logger.LogError(ex, "An error occured when executing a pre-launch action");
    } finally {
      scope.Dispose(); 
    }
  }
}
