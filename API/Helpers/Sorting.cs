namespace API.Helpers;

public enum UserSortOrder { KnownAs, DateOfBirth, CreatedAt, LastActive }
 
public interface ISortableUser {
  public string KnownAs { get; }
  public DateOnly DateOfBirth { get; }
  public DateTime CreatedAt { get; }
  public DateTime LastActive { get; }
}

public static class SortingExtensions {
  public static IQueryable<T> Sort<T>(this IQueryable<T> query, UserSortOrder? sortOrder) where T : ISortableUser => sortOrder switch {
    UserSortOrder.KnownAs => query.OrderBy(u => u.KnownAs),
    UserSortOrder.DateOfBirth => query.OrderByDescending(u => u.DateOfBirth),
    UserSortOrder.CreatedAt => query.OrderByDescending(u => u.CreatedAt),
    UserSortOrder.LastActive => query.OrderByDescending(u => u.LastActive),
    _ => query
  };
}
