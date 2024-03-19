using AutoMapper;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDto;
using Pinionszek_API.Models.DTOs.GetDto;

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

            CreateMap<GetPrivatePaymentDto, GetSharedPaymentToFriendDto>()
                 .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => src));

            CreateMap<GetFriendDto, GetSharedPaymentToFriendDto>()
                .ForMember(dest => dest.TargetFriend, opt => opt.MapFrom(src => src));

            CreateMap<Payment, GetAssignedPaymentDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.PaymentStatus.Name))
                .ForMember(dest => dest.IdSharedPayment, opt => opt.MapFrom(src => src.SharedPayment.IdSharedPayment))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.DetailedCategory));

            CreateMap<GetAssignedPaymentDto, GetAssignedPaymentToUserDto>()
                 .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => src));

            CreateMap<GetFriendDto, GetAssignedPaymentToUserDto>()
                .ForMember(dest => dest.SourceFriend, opt => opt.MapFrom(src => src));
        }
    }
}
