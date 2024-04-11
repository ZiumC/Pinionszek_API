using Pinionszek_API.Utils.Attributions;
using System.ComponentModel.DataAnnotations;

namespace Pinionszek_API.Models.DTOs.PostDto
{
    public class PostPaymentDto
    {
        [MaxLength(80, ErrorMessage = "Payment name is too long")]
        [MinLength(3, ErrorMessage = "Payment name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Charge is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Charge can't be less or equal 0")]
        public decimal Charge { get; set; }

        [Required(ErrorMessage = "Refund is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Refound can't be less than 0")]
        public decimal Refund { get; set; }

        [MaxLength(350, ErrorMessage = "Message is too long")]
        public string Message { get; set; }

        [BeforeToday(ErrorMessage = "Date is past than today")]
        public DateTime? PaymentDate { get; set; }

        [Required(ErrorMessage = "Payment category is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Category id can't be less or equal 0")]
        public int IdDetailedCategory { get; set; }

        public int FriendTag { get; set; }

        public List<DateTime> Months { get; set; }
    }
}
