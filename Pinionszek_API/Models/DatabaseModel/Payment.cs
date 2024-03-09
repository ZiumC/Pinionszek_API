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
        public DateTime? PaidOn { get; set; }
        public DateTime PaymentAddedOn { get; set; }

        public int IdPaymentStatus { get; set; }
        [ForeignKey(nameof(IdPaymentStatus))]
        public virtual PaymentStatus PaymentStatus { get; set; }

        public int IdBudget { get; set; }
        [ForeignKey(nameof(IdBudget))]
        public virtual Budget Budget { get; set; }

        public int IdDetailedCategory { get; set; }
        [ForeignKey(nameof(IdDetailedCategory))]
        public virtual DetailedCategory DetailedCategory { get; set; }

        public virtual SharedPayment SharedPayment { get; set; }
    }
}
