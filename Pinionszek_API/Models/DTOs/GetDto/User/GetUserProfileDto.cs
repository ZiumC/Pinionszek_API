namespace Pinionszek_API.Models.DTOs.GetDto.User
{
    public class GetUserProfileDto
    {
        public int UserTag { get; set; }
        public DateTime RegisteredAt { get; set; }
        public string Email { get; set; }
    }
}
