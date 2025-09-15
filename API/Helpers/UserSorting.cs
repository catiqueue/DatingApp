using API.Entities;

namespace API.Helpers;

public enum UserSortOrder { Id, Username, KnownAs, DateOfBirth, CreatedAt, LastActive }

public static class UserSorters {
  public static IQueryable<User> Sort(this IQueryable<User> query, UserSortOrder? sortOrder) => sortOrder switch {
    UserSortOrder.Id => query.OrderBy(u => u.Id),
    UserSortOrder.Username => query.OrderBy(u => u.NormalizedUserName),
    UserSortOrder.KnownAs => query.OrderBy(u => u.KnownAs),
    UserSortOrder.DateOfBirth => query.OrderByDescending(u => u.DateOfBirth),
    UserSortOrder.CreatedAt => query.OrderByDescending(u => u.CreatedAt),
    UserSortOrder.LastActive => query.OrderByDescending(u => u.LastActive),
    _ => query
  };
}
