using AutoMapper;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDto.Payments;
using Pinionszek_API.Models.DTOs.GetDto.User;

namespace Pinionszek_API.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile() 
        {
            CreateMap<DetailedCategory, GetPaymentCategoryDto>()
                .ForMember(dest => dest.DetailedName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.GeneralName, opt => opt.MapFrom(src => src.GeneralCategory.Name));

            CreateMap<DetailedCategory, GetUserCategoryDto>()
                .ForMember(dest => dest.IdDetailedCategory, opt => opt.MapFrom(src => src.IdDetailedCategory))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<GeneralCategory, GetUserGeneralCategoryDto>()
                .ForMember(dest => dest.IdGeneralCategory, opt => opt.MapFrom(src => src.IdGeneralCategory))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault));
        }
    }
}
