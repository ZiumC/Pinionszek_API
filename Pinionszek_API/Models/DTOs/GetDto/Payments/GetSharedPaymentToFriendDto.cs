namespace Pinionszek_API.Models.DTOs.GetDto.Payments
{
    public class GetSharedPaymentToFriendDto
    {
        public GetPrivatePaymentDto Payment { get; set; }
        public GetPaymentFriendDto TargetFriend { get; set; }
    }
}
