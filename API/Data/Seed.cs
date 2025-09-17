using System.Text.Json;

using API.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public static class Seed {
  private static readonly JsonSerializerOptions SerializerOptions = new() { PropertyNameCaseInsensitive = true };
  
  public static async Task SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager) {
    if (await userManager.Users.AnyAsync()) return;
    
    var data = await File.ReadAllTextAsync("Data/UserSeedData.json");
    var users = JsonSerializer.Deserialize<List<User>>(data, SerializerOptions) ?? [];
    var roles = new Role[] { new() { Name = "User" }, new() { Name = "Admin" }, new() { Name = "Moderator" } };
    var admin = new User {
      UserName = "admin",
      KnownAs = "Admin",
      Gender = Gender.None,
      City = "New York",
      Country = "United States"
    };
    
    foreach (var role in roles) 
      await roleManager.CreateAsync(role);
    foreach (var user in users.Concat([admin])) {
      user.CreatedAt = DateTime.SpecifyKind(user.CreatedAt, DateTimeKind.Utc);
      user.LastActive = DateTime.SpecifyKind(user.LastActive, DateTimeKind.Utc);
      await CreateWithRoles(user, user.UserName == "admin" ? ["Admin", "Moderator"] : ["User"]);
    }

    return;
    
    async Task CreateWithRoles(User user, IEnumerable<string> roles) {
      await userManager.CreateAsync(user, "Pa$$w0rd");
      await userManager.AddToRolesAsync(user, roles);
    }
  }
}
