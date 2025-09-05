using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class DbUserRole : IdentityUserRole<uint> {
  public DbUser User { get; set; } = null!;
  public DbRole Role { get; set; } = null!;
}
