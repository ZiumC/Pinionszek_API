namespace Pinionszek_API.Models.DTOs.GetDto.Payments
{
    public class GetAssignedPaymentToUserDto
    {
        public GetAssignedPaymentDto Payment { get; set; }
        public GetPaymentFriendDto SourceFriend { get; set; }
    }
}
