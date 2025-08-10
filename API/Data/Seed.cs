using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

using API.Entities;

using Microsoft.EntityFrameworkCore;

namespace API.Data;

public sealed class Seed {
  private static readonly JsonSerializerOptions SerializerOptions = new() { PropertyNameCaseInsensitive = true };
  public static async Task SeedUsers(DataContext context) {
    if (await context.Users.AnyAsync()) return;
    var data = await File.ReadAllTextAsync("Data/UserSeedData.json");
    var users = JsonSerializer.Deserialize<List<DbUser>>(data, SerializerOptions) ?? [];
    foreach (var user in users) {
      using var encoder = new HMACSHA512();
      user.Username = user.Username.ToLower();
      user.Base64PasswordHash = Convert.ToBase64String(encoder.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd")));
      user.Base64PasswordSalt = Convert.ToBase64String(encoder.Key);

      context.Users.Add(user);
    }

    await context.SaveChangesAsync();
  }
}
