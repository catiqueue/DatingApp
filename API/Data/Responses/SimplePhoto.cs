namespace API.Data.Responses;

public class SimplePhoto {
  public uint Id { get; init; }
  public string Url { get; init; } = "";
  public bool IsMain { get; init; }
}
