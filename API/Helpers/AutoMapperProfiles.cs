using API.Data.DTOs;
using API.Data.Requests;
using API.Data.Responses;
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
    CreateMap<DbMessage, SimpleMessage>()
      .ForMember(message => message.SenderAvatarUrl, 
                  config => config.MapFrom(message => message.Sender.Photos.FirstOrDefault(x => x.IsMain)!.Url))
      .ForMember(message => message.RecipientAvatarUrl,
                  config => config.MapFrom(message => message.Recipient.Photos.FirstOrDefault(x => x.IsMain)!.Url));
  }
}
