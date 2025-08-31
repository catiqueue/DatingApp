using System.ComponentModel.DataAnnotations;

using API.Data.Repositories;

namespace API.Data.Requests;

public sealed record GetLikedListRequest([Required] LikedListType Predicate = LikedListType.LikedBy) : PaginatedRequestBase;
