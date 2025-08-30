using System.ComponentModel.DataAnnotations;

using API.Entities;
using API.Helpers;

namespace API.Data.Requests;

public sealed record RegisterRequest(
  [Required]
  [StringLengthRange(3, 24)]
  string Username,
  [Required]
  string KnownAs,
  [Required]
  UserGender Gender,
  [Required]
  DateOnly DateOfBirth,
  [Required]
  string Country,
  [Required]
  string City,
  [Required]
  [StringLengthRange(8, 64)]
  string Password);
