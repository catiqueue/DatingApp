using System.ComponentModel.DataAnnotations;

namespace API.Data.Requests;

public sealed record RegisterRequest([Required] string Username, [Required] string Password);
