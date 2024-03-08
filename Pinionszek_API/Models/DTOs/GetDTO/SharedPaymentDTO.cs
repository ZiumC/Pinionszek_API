namespace Pinionszek_API.Models.DTOs.GetDTO
{
    public class SharedPaymentDTO
    {
        public int IdPayment { get; set; }
        public string Name { get; set; }
        public decimal Charge { get; set; }
        public decimal Refund { get; set; }
        public string Message { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Status { get; set; }
        public CategoryDTO Category { get; set; }
        public DateTime? PaidOn { get; set; }
        public DateTime PaymentAddedOn { get; set; }
        public string SharredTo { get; set; }
    }
}
