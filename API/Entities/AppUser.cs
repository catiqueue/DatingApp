namespace API.Entities {
  public sealed class AppUser {
    public uint Id { get; set; }
    public required string Username { get; set; }
    // TODO: store hashes as byte arrays
    public required string PasswordHash { get; set; }
    public required string PasswordSalt { get; set; }
  }
}
