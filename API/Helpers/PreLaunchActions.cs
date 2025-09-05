using API.Data;
using API.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers;

public static class PreLaunchActions {
  public static async Task ApplyMigrations(IServiceScope scope) 
    => await scope.ServiceProvider.GetRequiredService<DataContext>().Database.MigrateAsync();
  public static Task SeedUsers(IServiceScope scope) 
    => Seed.SeedUsers(scope.ServiceProvider.GetRequiredService<UserManager<DbUser>>(), 
                      scope.ServiceProvider.GetRequiredService<RoleManager<DbRole>>());
  
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
