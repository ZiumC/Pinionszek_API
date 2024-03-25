using AutoMapper;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDto;
using Pinionszek_API.Models.DTOs.GetDto.Payments;

namespace Pinionszek_API.Profiles
{
    public class BudgetProfile : Profile
    {
        public BudgetProfile()
        {
            CreateMap<Budget, GetBudgetDto>()
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.BudgetStatus.Name));

            CreateMap<GetBudgetDto, GetBudgetSummaryDto>()
               .ForMember(dest => dest.Budget, opt => opt.MapFrom(src => src));
        }
    }
}
