using System.Linq.Expressions;

using API.Data.Responses;
using API.Entities;
using API.Helpers;
using API.Services.Abstractions.Repositories;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public enum LikedListType { Liked, LikedBy, Mutual }

public sealed class LikeRepository(DataContext db, IMapper mapper) : ILikesRepository {
  public Task<int> CountAsync(LikedListType request, uint userId) => ReadOnlyLikes.FilterUsers(request, userId).CountAsync();

  public void AddLike(DbUserLike like) => db.Likes.Add(like);

  public void DeleteLike(DbUserLike like) => db.Likes.Remove(like);

  public async Task<DbUserLike?> GetDbLikeAsync(uint likingUserId, uint likedUserId)
    => await db.Likes.FindAsync(likingUserId, likedUserId);

  public async Task<IEnumerable<SimpleUser>> GetSimpleUserLikedListAsync(Page page, LikedListType request, uint userId) 
    => await ReadOnlyLikes
      .FilterUsers(request, userId)
      .Slice(page)
      .ProjectTo<SimpleUser>(mapper.ConfigurationProvider)
      .ToListAsync();

  public async Task<IEnumerable<uint>> GetUsersLikedIdsAsync(uint currentUserId)
    => await ReadOnlyLikes
      .Where(LikingUserIs(currentUserId))
      .Select(x => x.LikedUserId)
      .ToListAsync();
  
  private static Expression<Func<DbUserLike, bool>> LikingUserIs(uint userId) => like => like.LikingUserId == userId;
  private static Expression<Func<DbUserLike, bool>> LikedUserIs(uint userId) => like => like.LikedUserId == userId;
  private IQueryable<DbUserLike> ReadOnlyLikes => db.Likes.AsNoTracking();
}

file static class LikeRepositoryExtensions {
  public static IQueryable<DbUser> FilterUsers(this IQueryable<DbUserLike> likes, LikedListType type, uint userId) => type switch {
    LikedListType.LikedBy => likes.Where(like => like.LikedUserId == userId).Select(x => x.LikingUser),
    LikedListType.Liked => likes.Where(like => like.LikingUserId == userId).Select(x => x.LikedUser),
    // tough shit
    LikedListType.Mutual => likes
      .Where(like => like.LikingUserId == userId)
      .Join(likes,
        l => new { A = l.LikingUserId, B = l.LikedUserId },
        r => new { A = r.LikedUserId, B = r.LikingUserId },
        (l, r) => l.LikedUser
      ),
    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
  };
}
