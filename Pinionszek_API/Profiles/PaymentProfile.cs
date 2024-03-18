using AutoMapper;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDTO;

namespace Pinionszek_API.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, GetPrivatePaymentDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.PaymentStatus.Name))
                .ForMember(dest => dest.IdSharedPayment, opt => opt.MapFrom(src => src.SharedPayment.IdSharedPayment))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.DetailedCategory));

            CreateMap<GetPrivatePaymentDto, GetSharedPaymentDto>()
                 .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => src));

            CreateMap<GetFriendDto, GetSharedPaymentDto>()
                .ForMember(dest => dest.Friend, opt => opt.MapFrom(src => src));
        }
    }
}
