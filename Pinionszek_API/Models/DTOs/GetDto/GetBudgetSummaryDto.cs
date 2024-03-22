namespace Pinionszek_API.Models.DTOs.GetDto
{
    public class GetBudgetSummaryDto
    {
        public GetBudgetDto Budget { get; set; }
        public decimal Needs { get; set; }
        public decimal Wants { get; set; }
        public decimal Savings { get; set; }
        public decimal Actual { get; set; }
    }
}
