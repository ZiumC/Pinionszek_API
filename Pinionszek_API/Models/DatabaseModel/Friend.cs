using System.ComponentModel.DataAnnotations.Schema;

namespace Pinionszek_API.Models.DatabaseModel
{
    public class Friend
    {
        public int IdFriend { get; set; }
        public int FriendTag { get; set; }
        public DateTime DateAdded { get; set; }

        public int IdUser { get; set; }
        [ForeignKey(nameof(IdUser))]
        public virtual User User { get; set; }
    }
}
