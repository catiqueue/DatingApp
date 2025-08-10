using API.Data.DTOs;
using API.Entities;

namespace API.Helpers;

public class AutoMapperProfiles : AutoMapper.Profile {
  public AutoMapperProfiles() {
    CreateMap<DbUser, SimpleUser>();
    CreateMap<DbPhoto, SimplePhoto>();
  }
}
