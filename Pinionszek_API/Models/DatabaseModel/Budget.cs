using System.ComponentModel.DataAnnotations.Schema;

namespace Pinionszek_API.Models.DatabaseModel
{
    public class Budget
    {
        public int IdBudget { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? OpendDate { get; set; }
        public decimal Revenue { get; set; }
        public decimal Surplus { get; set; }
        public DateTime BudgetYear { get; set; }

        public int IdBudgetStatus { get; set; }
        [ForeignKey(nameof(IdBudgetStatus))]
        public virtual BudgetStatus BudgetStatus { get; set; }

        public int IdUser { get; set; }
        [ForeignKey(nameof(IdUser))]
        public virtual User User { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
