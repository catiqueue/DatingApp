using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public sealed class DbGroupConnection {
  [Key]
  public required string ConnectionId { get; set; }
  public required string Username { get; set; }
}
