namespace Pinionszek_API.Models.DTOs.GetDto
{
    public class GetUserSettingsDto
    {
        public int IdUserSetting { get; set; }
        public bool UseBudgetRules { get; set; }
        public bool DisplayBudgetRules { get; set; }
        public decimal NeedsRule { get; set; }
        public decimal WantsRule { get; set; }
        public decimal SavingsRule { get; set; }
    }
}
