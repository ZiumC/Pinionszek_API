namespace Pinionszek_API.Models.DTOs.GetDTO
{
    public class GetSharedPaymentDto
    {
        public GetPrivatePaymentDto Payment { get; set; }
        public GetFriendDto Friend { get; set; }
    }
}
