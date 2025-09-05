using System.ComponentModel.DataAnnotations.Schema;

using API.Extensions;
using API.Helpers;

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
  public List<DbPhoto> Photos { get; set; } = [];
  public List<DbUserLike> LikedBy { get; set; } = [];
  public List<DbUserLike> Likes { get; set; } = [];
  public List<DbMessage> MessagesSent { get; set; } = [];
  public List<DbMessage> MessagesReceived { get; set; } = [];
  public List<DbUserRole> UserRoles { get; set; } = [];
}
