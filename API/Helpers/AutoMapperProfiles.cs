using API.DTO.Requests;
using API.DTO.Responses;
using API.Entities;

namespace API.Helpers;

public class AutoMapperProfiles : AutoMapper.Profile {
  public AutoMapperProfiles() {
    CreateMap<RegisterRequest, User>();
      // .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username.ToLower()));
    CreateMap<UpdateUserRequest, User>();

    CreateMap<User, UserDto>()
      .ForMember(dest => dest.Roles, config => config.MapFrom(src => src.UserRoles.Select(role => role.Role.Name)));
    CreateMap<User, AuthenticatedUserDto>();
    CreateMap<Photo, PhotoDto>();
    CreateMap<Photo, UnapprovedPhotoDto>()
      .ForMember(dest => dest.Username, config => config.MapFrom(src => src.User.UserName));
    CreateMap<Message, MessageDto>()
      .ForMember(message => message.SenderAvatarUrl, 
                  config => config.MapFrom(message => message.Sender.Photos.FirstOrDefault(x => x.IsMain)!.Url))
      .ForMember(message => message.RecipientAvatarUrl,
                  config => config.MapFrom(message => message.Recipient.Photos.FirstOrDefault(x => x.IsMain)!.Url));

    CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
    CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) 
                                                                   : null);
    
    
  }
}
