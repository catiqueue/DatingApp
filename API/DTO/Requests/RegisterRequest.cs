using System.ComponentModel.DataAnnotations;

using API.Entities;
using API.Helpers;

namespace API.DTO.Requests;

public sealed class RegisterRequest {
  public required string Username { get; set; }
  public required string KnownAs { get; set; }
  public required Gender Gender { get; set; }
  public required DateOnly DateOfBirth { get; set; }
  public required string Country { get; set; }
  public required string City { get; set; }
  public required string Password { get; set; }
}
