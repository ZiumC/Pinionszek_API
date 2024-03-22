namespace Pinionszek_API.Models.DTOs.GetDto.Payments
{
    public class GetSharedPaymentToFriendDto
    {
        public GetPrivatePaymentDto Payment { get; set; }
        public GetFriendDto TargetFriend { get; set; }
    }
}
