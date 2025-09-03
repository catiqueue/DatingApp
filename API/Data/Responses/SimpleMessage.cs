namespace API.Data.Responses;

public class SimpleMessage {
  public uint Id { get; set; }
  
  public uint SenderId { get; set; }
  public required string SenderUsername { get; set; }
  public required string SenderAvatarUrl { get; set; }
  
  public uint RecipientId { get; set; }
  public required string RecipientUsername { get; set; }
  public required string RecipientAvatarUrl { get; set; }
  
  public required string Content { get; set; }
  
  public DateTime? ReadAt { get; set; }
  public DateTime SentAt { get; set; }
}
