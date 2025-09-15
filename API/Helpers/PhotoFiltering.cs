using API.Entities;

namespace API.Helpers;

public enum PhotoApprovalStatus { Any, Pending, Approved }

public sealed record PhotoFilter(
    int? UserId = null,
    PhotoApprovalStatus ApprovalStatus = PhotoApprovalStatus.Any,
    bool OnlyMain = false,
    bool? HasPublicId = null) {
  public static PhotoFilter Empty { get; } = new();
  public static PhotoFilter FromStatus(PhotoApprovalStatus status) => new(ApprovalStatus: status);
  public static PhotoFilter ForUserId(int userId) => new(userId);
};

public static class PhotoFilters {
  public static IQueryable<Photo> Filter(this IQueryable<Photo> query, PhotoFilter filter) 
    => query.FilterApprovalStatus(filter.ApprovalStatus)
            .FilterMain(filter.OnlyMain)
            .FilterPublicId(filter.HasPublicId);
  
  private static IQueryable<Photo> FilterUserId(this IQueryable<Photo> query, int? userId) 
    => userId is not { } value
      ? query
      : query.Where(p => p.UserId == value);
  
  private static IQueryable<Photo> FilterApprovalStatus(this IQueryable<Photo> query, PhotoApprovalStatus status) => status switch {
    PhotoApprovalStatus.Pending => query.Where(p => !p.IsApproved),
    PhotoApprovalStatus.Approved => query.Where(p => p.IsApproved),
    _ => query
  };
  private static IQueryable<Photo> FilterMain(this IQueryable<Photo> query, bool onlyMain) => onlyMain ? query.Where(p => p.IsMain) : query;
  private static IQueryable<Photo> FilterPublicId(this IQueryable<Photo> query, bool? hasPublicId) 
    => hasPublicId is not { } value 
      ? query 
      : value switch {
        true => query.Where(p => p.PublicId != null), 
        false => query.Where(p => p.PublicId == null)
      };
}
