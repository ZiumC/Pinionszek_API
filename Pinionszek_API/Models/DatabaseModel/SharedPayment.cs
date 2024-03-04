using System.ComponentModel.DataAnnotations.Schema;

namespace Pinionszek_API.Models.DatabaseModel
{
    public class SharedPayment
    {
        public int IdSharedPayment { get; set; }

        public int IdPayment { get; set; }
        [ForeignKey(nameof(IdPayment))]
        public virtual Payment Payment { get; set; }

        public int IdFriend { get; set; }
        [ForeignKey(nameof(IdFriend))]
        public virtual Friend Friend { get; set; }
    }
}
