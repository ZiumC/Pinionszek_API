using AutoMapper;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDto.User;

namespace Pinionszek_API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<Friend, GetUserFriendDto>()
                   .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.User.Login));
                   
        }
    }
}
