using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public sealed class DbUser : IdentityUser<uint> {
  public DateOnly DateOfBirth { get; set; }
  public required string KnownAs { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime LastActive { get; set; } = DateTime.UtcNow;
  public required UserGender Gender { get; set; }
  public string? Introduction { get; set; }
  public string? Interests { get; set; }
  public string? LookingFor { get; set; }
  public required string City { get; set; }
  public required string Country { get; set; }
  public ICollection<DbPhoto> Photos { get; set; } = [];
  public ICollection<DbUserLike> LikedBy { get; set; } = [];
  public ICollection<DbUserLike> Likes { get; set; } = [];
  public ICollection<DbMessage> MessagesSent { get; set; } = [];
  public ICollection<DbMessage> MessagesReceived { get; set; } = [];
  public ICollection<DbUserRole> UserRoles { get; set; } = [];
}
