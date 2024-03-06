using AutoMapper;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDTO;

namespace Pinionszek_API.Profiles
{
    public class PaymentProfile : Profile 
    {
        public PaymentProfile() 
        {
            CreateMap<GetPaymentDTO, Payment>()
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.DetailedCategory, opt => opt.MapFrom(src => src.DetailedCategory));

            CreateMap<Payment, GetPaymentDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.PaymentStatus.Name))
                .ForMember(dest => dest.DetailedCategory, opt => opt.MapFrom(src => src.DetailedCategory.Name));
        }
    }
}
