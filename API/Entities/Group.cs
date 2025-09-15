using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public sealed class Group {
  [Key]
  public required string Name { get; set; }
  public ICollection<GroupConnection> Connections { get; set; } = [];
}
