using AutoMapper;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDto.User;

namespace Pinionszek_API.Profiles
{
    public class UserSettingsProfile : Profile
    {
        public UserSettingsProfile()
        {
            CreateMap<UserSettings, GetUserSettingsDto>()
                   .ForMember(dest => dest.NeedsRule, opt => opt.MapFrom(src => src.Needs))
                   .ForMember(dest => dest.WantsRule, opt => opt.MapFrom(src => src.Wants))
                   .ForMember(dest => dest.SavingsRule, opt => opt.MapFrom(src => src.Savings));
        }
    }
}
