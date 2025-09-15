using API.Entities;
using API.Extensions;

namespace API.DTO.Responses;

public class UserDto {
  public int Id { get; init; }
  public required string UserName { get; init; }
  public DateOnly DateOfBirth { get; init; }
  public int Age =>  DateOfBirth.GetAge(now: DateOnly.FromDateTime(DateTime.UtcNow));
  public string? AvatarUrl => Photos.FirstOrDefault(ph => ph.IsMain)?.Url;
  public required string KnownAs { get; init; }
  public DateTime CreatedAt { get; init; }
  public DateTime LastActive { get; init; }
  public required Gender Gender { get; init; }
  public string? Introduction { get; init; }
  public string? Interests { get; init; }
  public string? LookingFor { get; init; }
  public required string City { get; init; }
  public required string Country { get; init; }
  public IEnumerable<PhotoDto> Photos { get; init; } = [];
  public IEnumerable<string> Roles { get; init; } = [];
}
