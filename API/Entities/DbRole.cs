using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class DbRole : IdentityRole<uint> {
  public ICollection<DbUserRole> UserRoles { get; set; } = [];
}
