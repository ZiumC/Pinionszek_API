using System.ComponentModel.DataAnnotations.Schema;

namespace Pinionszek_API.Models.DatabaseModel
{
    public class Payment
    {
        public int IdPayment { get; set; }
        public string Name { get; set; }
        public decimal Charge { get; set; }
        public decimal Refund { get; set; }
        public string Message { get; set; }
        public DateTime? PaymentDate { get; set; }

        public int IdPaymentStatus { get; set; }
        [ForeignKey(nameof(IdPaymentStatus))]
        public virtual PaymentStatus PaymentStatus { get; set; }
    }
}
