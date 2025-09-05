using API.Entities;

namespace API.Helpers;

public sealed record UserFilter(UserGender? Gender, int? MinAge, int? MaxAge, IEnumerable<string> SkippedUsernames) {
  public static UserFilter FromSkippedUsername(string skippedUsername) => new(null, null, null, new[] { skippedUsername });
}

public static class FilteringExtensions {
  public static IQueryable<DbUser> Filter(this IQueryable<DbUser> query, UserFilter filter) {
    if(filter.Gender is not null) query = query.FilterGender(filter.Gender.Value);
    if(filter.MinAge is not null) query = query.FilterMinAge(filter.MinAge.Value);
    if(filter.MaxAge is not null) query = query.FilterMaxAge(filter.MaxAge.Value);
    query = query.SkipUsernames(filter.SkippedUsernames);
    return query;
  }

  private static IQueryable<DbUser> FilterGender(this IQueryable<DbUser> query, UserGender requestedGender) 
    => query.Where(u => u.Gender == requestedGender);
  private static IQueryable<DbUser> FilterMinAge(this IQueryable<DbUser> query, int minAge) 
    => query.Where(u => u.DateOfBirth <= DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-minAge));
  private static IQueryable<DbUser> FilterMaxAge(this IQueryable<DbUser> query, int maxAge) 
    => query.Where(u => u.DateOfBirth >= DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-maxAge-1));
  private static IQueryable<DbUser> SkipUsernames(this IQueryable<DbUser> query, IEnumerable<string> skippedUsernames) 
    => query.Where(u => !skippedUsernames.Select(uname => uname.ToLower()).Contains(u.UserName));
}
