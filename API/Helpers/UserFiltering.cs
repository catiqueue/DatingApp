using API.Entities;

namespace API.Helpers;

public sealed record UserFilter(Gender? Gender, int? MinAge, int? MaxAge, IEnumerable<string> SkippedUsernames) {
  public static UserFilter Empty { get; } = new(null, null, null, []);
  public static UserFilter FromSkippedUsername(string skippedUsername) => new(null, null, null, new[] { skippedUsername });
}

public static class UserFilters {
  public static IQueryable<User> Filter(this IQueryable<User> query, UserFilter filter) {
    if(filter.Gender is not null) query = query.FilterGender(filter.Gender.Value);
    if(filter.MinAge is not null) query = query.FilterMinAge(filter.MinAge.Value);
    if(filter.MaxAge is not null) query = query.FilterMaxAge(filter.MaxAge.Value);
    query = query.SkipUsernames(filter.SkippedUsernames);
    return query;
  }

  private static IEnumerable<string> InternalUsernames { get; } = ["admin"];  
  private static IQueryable<User> FilterGender(this IQueryable<User> query, Gender requestedGender) 
    => query.Where(u => u.Gender == requestedGender);
  private static IQueryable<User> FilterMinAge(this IQueryable<User> query, int minAge) 
    => query.Where(u => u.DateOfBirth <= DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-minAge));
  private static IQueryable<User> FilterMaxAge(this IQueryable<User> query, int maxAge) 
    => query.Where(u => u.DateOfBirth >= DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-maxAge-1));
  private static IQueryable<User> SkipUsernames(this IQueryable<User> query, IEnumerable<string> skippedUsernames) 
    => query.Where(u => !skippedUsernames.Concat(InternalUsernames).Select(uname => uname.ToUpperInvariant()).Contains(u.NormalizedUserName));
}
