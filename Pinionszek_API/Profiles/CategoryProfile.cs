using AutoMapper;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDto;

namespace Pinionszek_API.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile() 
        {
            CreateMap<DetailedCategory, GetCategoryDto>()
                .ForMember(dest => dest.DetailedName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.GeneralName, opt => opt.MapFrom(src => src.GeneralCategory.Name));
        }
    }
}
