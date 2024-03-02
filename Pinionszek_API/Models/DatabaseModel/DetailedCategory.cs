using System.ComponentModel.DataAnnotations.Schema;

namespace Pinionszek_API.Models.DatabaseModel
{
    public class DetailedCategory
    {
        public int IdDetailedCategory { get; set; }
        public string Name { get; set; }

        public int IdGeneralCategory { get; set; }
        [ForeignKey(nameof(IdGeneralCategory))]
        public virtual GeneralCategory GeneralCategory { get; set; }
    }
}
