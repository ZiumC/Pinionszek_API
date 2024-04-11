using System.ComponentModel.DataAnnotations;

namespace Pinionszek_API.Models.DTOs.PostDto
{
    public class PostPaymentDto
    {
        [Required(ErrorMessage = "Payment name is required")]
        [MaxLength(80, ErrorMessage = "Payment name is too long")]
        [MinLength(3, ErrorMessage = "Payment name is too short")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Charge is required")]
        public decimal Charge { get; set; }

        [Required(ErrorMessage = "Refund is required")]
        public decimal Refund { get; set; }

        [MaxLength(350, ErrorMessage = "Message is too long")]
        public string Message { get; set; }

        public DateTime? PaymentDate { get; set; }

        [Required(ErrorMessage = "Payment category is required")]
        public int IdDetailedCategory { get; set; }

        public int FriendTag { get; set; }

        public List<DateTime> Months { get; set; }
    }
}
