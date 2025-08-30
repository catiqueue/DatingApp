using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Photos")]
public sealed class DbPhoto : DbEntityBase {
  public required string Url { get; set; }
  public bool IsMain { get; set; }
  public string? PublicId { get; set; }
  
  public uint DbUserId { get; set; }
  public DbUser DbUser { get; set; } = null!;
}
