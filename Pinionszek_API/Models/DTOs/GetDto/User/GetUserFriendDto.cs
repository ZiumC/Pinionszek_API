namespace Pinionszek_API.Models.DTOs.GetDto.User
{
    public class GetUserFriendDto
    {
        public int IdFriend { get; set; }
        public int FriendTag { get; set; }
        public string Login { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
