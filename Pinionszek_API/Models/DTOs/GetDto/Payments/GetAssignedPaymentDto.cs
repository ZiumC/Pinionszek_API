namespace Pinionszek_API.Models.DTOs.GetDto.Payments
{
    public class GetAssignedPaymentDto
    {
        public int IdPayment { get; set; }
        public int IdSharedPayment { get; set; }
        public string Name { get; set; }
        public decimal Charge { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public GetPaymentCategoryDto Category { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
