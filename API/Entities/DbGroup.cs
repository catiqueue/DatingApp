using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public sealed class DbGroup {
  [Key]
  public required string Name { get; set; }
  public ICollection<DbGroupConnection> Connections { get; set; } = [];
}
