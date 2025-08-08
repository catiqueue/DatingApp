using System.ComponentModel.DataAnnotations;

namespace API.Data.Requests;

public sealed record RegisterRequest(
  [Required] 
  // [MinLength(3)] [MaxLength(24)]
  [StringLength(maximumLength: 24, MinimumLength = 3)]
  string Username = "",
  [Required]
  // [MinLength(12)] [MaxLength(64)]
  [StringLength(maximumLength: 64, MinimumLength = 12)]
  string Password = "");
