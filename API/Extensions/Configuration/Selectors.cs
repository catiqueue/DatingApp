namespace API.Extensions.Configuration;

public static class Selectors {
  public const string JwtTokenKeySelector = "TokenKey";
  public const string SqliteConnectionSelector = "ConnectionStrings:SqliteConnection";
  public const string PostgreSqlConnectionSelector = "ConnectionStrings:PostgreSqlConnection";
  public const string CloudinarySettingsSelector = "CloudinarySettings";
}
