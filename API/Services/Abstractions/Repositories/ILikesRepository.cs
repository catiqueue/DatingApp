using API.Data.Repositories;
using API.Data.Responses;
using API.Entities;
using API.Helpers;

namespace API.Services.Abstractions.Repositories;

public interface ILikesRepository : IRepository {
  Task<int> CountAsync(LikedListType request, uint userId);
  void AddLike(DbUserLike like);
  void DeleteLike(DbUserLike like);
  Task<DbUserLike?> GetDbLikeAsync(uint likingUserId, uint likedUserId);
  Task<IEnumerable<SimpleUser>> GetSimpleUserLikedListAsync(Page page, LikedListType request, uint userId);
  Task<IEnumerable<uint>> GetUsersLikedIdsAsync(uint currentUserId);
}
