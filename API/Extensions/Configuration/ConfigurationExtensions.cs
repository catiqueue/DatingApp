namespace API.Extensions.Configuration;

public static class ConfigurationExtensions {
  public static string GetJwtSymmetricalKey(this IConfiguration configuration) 
    => configuration.GetSection(Selectors.JwtTokenKeySelector).Value is {} tokenKey
      ? tokenKey.Length >= 64 
        ? tokenKey 
        : throw new InvalidOperationException($"{Selectors.JwtTokenKeySelector} must be at least 64 characters long.")
      : throw new InvalidOperationException($"Missing configuration value: {Selectors.JwtTokenKeySelector}");
  
  public static string GetSqliteConnectionString(this IConfiguration configuration) 
    => configuration.GetSection(Selectors.SqliteConnectionSelector).Value 
       ?? throw new InvalidOperationException($"Missing configuration value: {Selectors.SqliteConnectionSelector}");
  
  public static IConfigurationSection GetCloudinarySection(this IConfiguration configuration) 
    => configuration.GetSection(Selectors.CloudinarySettingsSelector);
}
