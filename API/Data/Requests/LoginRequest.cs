using System.ComponentModel.DataAnnotations;

namespace API.Data.Requests;

public sealed record LoginRequest([Required] string Username, [Required] string Password);
