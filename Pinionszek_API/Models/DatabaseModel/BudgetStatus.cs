namespace Pinionszek_API.Models.DatabaseModel
{
    public class BudgetStatus
    {
        public int IdBudgetStatus { get; set; }
        public string Name { get; set; }
        public virtual Budget Budget { get; set; }
    }
}
