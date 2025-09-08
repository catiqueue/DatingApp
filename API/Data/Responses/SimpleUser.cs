using API.Entities;
using API.Extensions;

namespace API.Data.Responses;

public class SimpleUser {
  public uint Id { get; init; }
  public string UserName { get; init; } = "";
  public DateOnly DateOfBirth { get; init; }
  public uint Age => DateOfBirth.GetAge(now: DateOnly.FromDateTime(DateTime.UtcNow));
  public string? AvatarUrl => Photos.FirstOrDefault(ph => ph.IsMain)?.Url;
  public string KnownAs { get; init; } = "";
  public DateTime CreatedAt { get; init; }
  public DateTime LastActive { get; init; }
  public UserGender Gender { get; init; }
  public string? Introduction { get; init; }
  public string? Interests { get; init; }
  public string? LookingFor { get; init; }
  public string City { get; init; } = "";
  public string Country { get; init; } = "";
  public IEnumerable<SimplePhoto> Photos { get; init; } = [];
  public IEnumerable<string> Roles { get; init; } = [];
}
