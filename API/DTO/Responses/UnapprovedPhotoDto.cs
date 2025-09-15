namespace API.DTO.Responses;

public class UnapprovedPhotoDto {
  public int Id { get; set; }
  public required string Url { get; set; }
  public required string Username { get; set; }
}
