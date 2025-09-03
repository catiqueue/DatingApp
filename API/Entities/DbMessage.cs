namespace API.Entities;

public class DbMessage : DbEntityBase {
  public required string SenderUsername { get; set; }
  public required string RecipientUsername { get; set; }
  public required string Content { get; set; }
  public DateTime? ReadAt { get; set; }
  public DateTime SentAt { get; set; } = DateTime.UtcNow;
  public bool SenderDeleted { get; set; }
  public bool RecipientDeleted { get; set; }
  
  public DbUser Sender { get; set; } = null!;
  public uint SenderId { get; set; }
  public DbUser Recipient { get; set; } = null!;
  public uint RecipientId { get; set; }
}
