using API.Entities;

namespace API.Helpers;

public interface IFilterableUser {
  string Username { get; }
  string KnownAs { get; }
  DateOnly DateOfBirth { get; }
  UserGender Gender { get; }
  string? Interests { get; }
  string? LookingFor { get; }
  string? City { get; }
  string? Country { get; }
}

public sealed record UserFilter(UserGender? Gender, int? MinAge, int? MaxAge, IEnumerable<string> SkippedUsernames) {
  public static UserFilter FromSkippedUsername(string skippedUsername) => new(null, null, null, new[] { skippedUsername });
}

public static class FilteringExtensions {
  public static IQueryable<T> Filter<T>(this IQueryable<T> query, UserFilter filter) where T : IFilterableUser {
    // omg bro
    if(filter.Gender is not null) query = query.FilterGender(filter.Gender.Value);
    if(filter.MinAge is not null) query = query.FilterMinAge(filter.MinAge.Value);
    if(filter.MaxAge is not null) query = query.FilterMaxAge(filter.MaxAge.Value);
    query = query.SkipUsernames(filter.SkippedUsernames);
    return query;
  }

  private static IQueryable<T> FilterGender<T>(this IQueryable<T> query, UserGender requestedGender) where T : IFilterableUser 
    => query.Where(u => u.Gender == requestedGender);
  private static IQueryable<T> FilterMinAge<T>(this IQueryable<T> query, int minAge) where T : IFilterableUser 
    => query.Where(u => u.DateOfBirth <= DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-minAge));
  private static IQueryable<T> FilterMaxAge<T>(this IQueryable<T> query, int maxAge) where T : IFilterableUser 
    => query.Where(u => u.DateOfBirth >= DateOnly.FromDateTime(DateTime.UtcNow).AddYears(-maxAge-1));
  private static IQueryable<T> SkipUsernames<T>(this IQueryable<T> query, IEnumerable<string> skippedUsernames) where T : IFilterableUser 
    => query.Where(u => !skippedUsernames.Select(uname => uname.ToLower()).Contains(u.Username));
}
