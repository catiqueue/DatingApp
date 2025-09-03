using API.Data.Repositories;
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
public class LikesController(ILikesRepository likes) : ApiControllerBase {
  [HttpPost("{liked:int}")]
  public async Task<ActionResult> ToggleLike(uint liked) {
    var liking = User.GetId();
    
    if(liking == liked) return BadRequest("You can't like yourself. (sorry)");

    if (await likes.GetDbLikeAsync(liking, liked) is not { } existingLike) {
      likes.AddLike(new DbUserLike { LikingUserId = liking, LikedUserId = liked });
    } else {
      likes.DeleteLike(existingLike);
    }

    return await likes.TrySaveAllAsync() ? Ok() : BadRequest("Failed to update the like.");
  }
  
  [HttpGet("list")]
  public async Task<ActionResult<IEnumerable<uint>>> GetLikedUserIds() => Ok(await likes.GetUsersLikedIdsAsync(User.GetId()));
  
  [HttpGet]
  public async Task<ActionResult<PaginatedResponse<SimpleUser>>> GetLikeList([FromQuery] GetLikedListRequest request) 
    => PaginationInfo.TryCreate(request.ToPage(), await likes.CountAsync(request.Predicate, User.GetId()), out var paginationInfo) 
      ? Ok(PaginatedResponse<SimpleUser>.FromPaginationInfo(
          paginationInfo,
          await likes.GetSimpleUserLikedListAsync(request.ToPage(), request.Predicate, User.GetId()))) 
      : request.PageNumber == 1
        ? Ok(PaginatedResponse<SimpleUser>.Empty(request.PageSize))
        : BadRequest($"Page {request.PageNumber} does not exist.");
}
