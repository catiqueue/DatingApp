using API.Data.Requests;
using API.Data.Responses;
using API.Entities;

namespace API.Helpers;

public class AutoMapperProfiles : AutoMapper.Profile {
  public AutoMapperProfiles() {
    CreateMap<RegisterRequest, DbUser>();
      // .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username.ToLower()));
    CreateMap<UpdateUserRequest, DbUser>();

    CreateMap<DbUser, SimpleUser>()
      .ForMember(dest => dest.Roles, config => config.MapFrom(src => src.UserRoles.Select(role => role.Role.Name)));
    CreateMap<DbUser, AuthenticatedUser>();
    CreateMap<DbPhoto, SimplePhoto>();
    CreateMap<DbMessage, SimpleMessage>()
      .ForMember(message => message.SenderAvatarUrl, 
                  config => config.MapFrom(message => message.Sender.Photos.FirstOrDefault(x => x.IsMain)!.Url))
      .ForMember(message => message.RecipientAvatarUrl,
                  config => config.MapFrom(message => message.Recipient.Photos.FirstOrDefault(x => x.IsMain)!.Url));

    CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
    CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) 
                                                                   : null);
    
  }
}
