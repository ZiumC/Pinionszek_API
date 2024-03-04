using System.ComponentModel.DataAnnotations.Schema;

namespace Pinionszek_API.Models.DatabaseModel
{
    public class UserSettings
    {
        public int IdUserSetting { get; set; }
        public bool UseBudgetRules { get; set; }
        public bool DisplayBudgetRules { get; set; }

        public decimal Needs { get; set; }
        public decimal Wants { get; set; }
        public decimal Savings { get; set; }

        public int IdUser { get; set; }
        [ForeignKey(nameof(IdUser))]
        public virtual User User { get; set; }
    }
}
