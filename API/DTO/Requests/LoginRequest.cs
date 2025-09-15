using System.ComponentModel.DataAnnotations;

namespace API.DTO.Requests;

public sealed class LoginRequest {
  public required string Username { get; set; } 
  public required string Password { get; set; } 
}
