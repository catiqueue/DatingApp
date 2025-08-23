using API.Data.DTOs;
using API.Data.Requests;
using API.Entities;

namespace API.Helpers;

public class AutoMapperProfiles : AutoMapper.Profile {
  public AutoMapperProfiles() {
    CreateMap<RegisterRequest, DbUser>()
      .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username.ToLower()));
    CreateMap<UpdateUserRequest, DbUser>();
    
    CreateMap<DbUser, SimpleUser>();
    CreateMap<DbUser, AuthenticatedUser>();
    CreateMap<DbPhoto, SimplePhoto>();
    
  }
}
