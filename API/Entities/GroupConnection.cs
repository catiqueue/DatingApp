using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public sealed class GroupConnection {
  [Key]
  public required string ConnectionId { get; set; }
  public required string Username { get; set; }
}
