namespace Pinionszek_API.Models.DTOs.GetDto
{
    public class GetBudgetDto
    {
        public int IdBudget { get; set; }
        public string Status { get; set; }
        public DateTime? OpendDate { get; set; }
        public DateTime BudgetYear { get; set; }
        public decimal Revenue { get; set; }
        public decimal Surplus { get; set; }
        public bool IsCompleted { get; set; }
    }
}
