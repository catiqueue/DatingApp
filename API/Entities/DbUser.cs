using System.ComponentModel.DataAnnotations.Schema;

using API.Extensions;
using API.Helpers;

namespace API.Entities;

public sealed class DbUser : DbEntityBase, IFilterableUser, ISortableUser {
  // public uint Id { get; set; }
  public required string Username { get; set; }
  public string Base64PasswordHash { get; set; } = "";
  public string Base64PasswordSalt { get; set; } = "";
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
}
