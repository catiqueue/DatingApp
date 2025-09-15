namespace API.Entities;

public class Message : EntityBase<Message> {
  public required string SenderUsername { get; set; }
  public required string RecipientUsername { get; set; }
  public required string Content { get; set; }
  
  public DateTime? ReadAt { get; set; }
  public DateTime SentAt { get; set; } = DateTime.UtcNow;
  
  public bool SenderDeleted { get; set; }
  public bool RecipientDeleted { get; set; }
  
  public User Sender { get; set; } = null!;
  public int SenderId { get; set; }
  public User Recipient { get; set; } = null!;
  public int RecipientId { get; set; }
}
