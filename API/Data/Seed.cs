using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

using API.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public static class Seed {
  private static readonly JsonSerializerOptions SerializerOptions = new() { PropertyNameCaseInsensitive = true };
  
  public static async Task SeedUsers(UserManager<DbUser> userManager, RoleManager<DbRole> roleManager) {
    if (await userManager.Users.AnyAsync()) return;
    
    var data = await File.ReadAllTextAsync("Data/UserSeedData.json");
    var users = JsonSerializer.Deserialize<List<DbUser>>(data, SerializerOptions) ?? [];
    var roles = new DbRole[] { new() { Name = "User" }, new() { Name = "Admin" }, new() { Name = "Moderator" } };
    var admin = new DbUser {
      UserName = "admin",
      KnownAs = "Admin",
      Gender = UserGender.Other,
      City = "New York",
      Country = "United States"
    };
    
    foreach (var role in roles) 
      await roleManager.CreateAsync(role);
    foreach (var user in users.Concat([admin])) 
      await CreateWithRoles(user, user.UserName == "admin" ? ["Admin", "Moderator"] : ["User"]);

    return;
    
    async Task CreateWithRoles(DbUser user, IEnumerable<string> roles) {
      await userManager.CreateAsync(user, "Pa$$w0rd");
      await userManager.AddToRolesAsync(user, roles);
    }
  }
}
