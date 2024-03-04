namespace Pinionszek_API.Models.DatabaseModel
{
    public class User
    {
        public int IdUser { get; set; }
        public int UserTag { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime RegisteredAt { get; set; }
        public string? RefreshToken { get; set; }
        public int LoginAttempts { get; set; }
        public DateTime? BlockedTo { get; set; }

        public virtual ICollection<Friend> Friends { get; set; }
        public virtual UserSettings UserSettings { get; set; }
        public virtual ICollection<DetailedCategory> UserCategories { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
    }
}
