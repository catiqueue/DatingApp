namespace API.DTO.Responses;

public class PhotoDto {
  public int Id { get; init; }
  public required string Url { get; init; }
  public bool IsMain { get; init; }
  public bool IsApproved { get; init; }
}
