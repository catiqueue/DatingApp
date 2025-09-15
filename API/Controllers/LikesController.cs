using API.DTO.Requests;
using API.DTO.Responses;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class LikesController(IUnitOfWork work) : ApiControllerBase {
  [HttpPost("{liked:int}")]
  public async Task<ActionResult> ToggleLike(int liked) {
    var liking = User.GetId();
    
    if(liking == liked) return BadRequest("You can't like yourself. (sorry)");

    if (await work.Likes.GetLikeAsync(liking, liked) is not { } existingLike) {
      work.Likes.AddLike(new UserLike { LikingUserId = liking, LikedUserId = liked });
    } else {
      work.Likes.DeleteLike(existingLike);
    }

    return await work.TrySaveAllAsync() ? Ok() : BadRequest("Failed to update the like.");
  }
  
  [HttpGet("list")]
  public async Task<ActionResult<IEnumerable<int>>> GetLikedUserIds() => Ok(await work.Likes.GetUsersLikedIdsAsync(User.GetId()));
  
  [HttpGet]
  public async Task<ActionResult<PaginatedResponseDto<UserDto>>> GetLikeList([FromQuery] GetLikedListRequest request) 
    => PaginationInfo.TryCreate(request.ToPage(), await work.Likes.CountAsync(request.Predicate, User.GetId()), out var paginationInfo) 
      ? Ok(PaginatedResponseDto<UserDto>.FromPaginationInfo(
          paginationInfo,
          await work.Likes.GetUserDtoLikedListAsync(request.ToPage(), request.Predicate, User.GetId()))) 
      : request.PageNumber == 1
        ? Ok(PaginatedResponseDto<UserDto>.Empty(request.PageSize))
        : BadRequest($"Page {request.PageNumber} does not exist.");
}
