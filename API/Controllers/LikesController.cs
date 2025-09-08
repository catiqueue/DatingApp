using API.Data.Requests;
using API.Data.Responses;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Services.Abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class LikesController(IUnitOfWork work) : ApiControllerBase {
  [HttpPost("{liked:int}")]
  public async Task<ActionResult> ToggleLike(uint liked) {
    var liking = User.GetId();
    
    if(liking == liked) return BadRequest("You can't like yourself. (sorry)");

    if (await work.Likes.GetDbLikeAsync(liking, liked) is not { } existingLike) {
      work.Likes.AddLike(new DbUserLike { LikingUserId = liking, LikedUserId = liked });
    } else {
      work.Likes.DeleteLike(existingLike);
    }

    return await work.TrySaveAllAsync() ? Ok() : BadRequest("Failed to update the like.");
  }
  
  [HttpGet("list")]
  public async Task<ActionResult<IEnumerable<uint>>> GetLikedUserIds() => Ok(await work.Likes.GetUsersLikedIdsAsync(User.GetId()));
  
  [HttpGet]
  public async Task<ActionResult<PaginatedResponse<SimpleUser>>> GetLikeList([FromQuery] GetLikedListRequest request) 
    => PaginationInfo.TryCreate(request.ToPage(), await work.Likes.CountAsync(request.Predicate, User.GetId()), out var paginationInfo) 
      ? Ok(PaginatedResponse<SimpleUser>.FromPaginationInfo(
          paginationInfo,
          await work.Likes.GetSimpleUserLikedListAsync(request.ToPage(), request.Predicate, User.GetId()))) 
      : request.PageNumber == 1
        ? Ok(PaginatedResponse<SimpleUser>.Empty(request.PageSize))
        : BadRequest($"Page {request.PageNumber} does not exist.");
}
