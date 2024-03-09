namespace Pinionszek_API.Models.DTOs.GetDTO
{
    public class SharedPaymentDTO
    {
        public PrivatePaymentDTO Payment { get; set; }
        public FriendDto Friend { get; set; }
    }
}
