using Microsoft.EntityFrameworkCore;
using Pinionszek_API.Models.DatabaseModel;

namespace Pinionszek_API.DbContexts
{
    public class ProdDbContext : DbContext
    {
        public ProdDbContext()
        {
        }

        public ProdDbContext(DbContextOptions opt) : base(opt)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(u => u.IdUser);
                user.Property(u => u.UserTag).IsRequired();
                user.Property(u => u.Email).IsRequired().HasMaxLength(50);
                user.Property(u => u.Login).IsRequired().HasMaxLength(50);
                user.Property(u => u.Password).IsRequired().HasMaxLength(75);
                user.Property(u => u.PasswordSalt).IsRequired().HasMaxLength(10);
                user.Property(u => u.RegisteredAt).IsRequired();
                user.Property(u => u.RefreshToken).HasMaxLength(255);
            });

            modelBuilder.Entity<Friend>(friend =>
            {
                friend.HasKey(f => f.IdFriend);
                friend.Property(f => f.DateAdded).IsRequired().HasColumnType("date");
                friend.Property(f => f.FriendTag).IsRequired();
            });

            modelBuilder.Entity<UserSettings>(userSettings =>
            {
                userSettings.HasKey(us => us.IdUserSetting);
                userSettings.Property(us => us.UseBudgetRules).IsRequired();
                userSettings.Property(us => us.DisplayBudgetRules).IsRequired();
                userSettings.Property(us => us.Needs).IsRequired().HasColumnType("money");
                userSettings.Property(us => us.Wants).IsRequired().HasColumnType("money");
                userSettings.Property(us => us.Savings).IsRequired().HasColumnType("money");
            });

            modelBuilder.Entity<GeneralCategory>(generalCategory =>
            {
                generalCategory.HasKey(gc => gc.IdGeneralCategory);
                generalCategory.Property(gc => gc.Name).IsRequired().HasMaxLength(80);
                generalCategory.Property(gc => gc.IsDefault).IsRequired();
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
    }
}
