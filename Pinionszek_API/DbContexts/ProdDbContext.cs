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

            modelBuilder.Entity<DetailedCategory>(detailedCategory =>
            {
                detailedCategory.HasKey(gc => gc.IdDetailedCategory);
                detailedCategory.Property(gc => gc.Name).IsRequired().HasMaxLength(80);
            });

            modelBuilder.Entity<PaymentStatus>(paymentStatus =>
            {
                paymentStatus.HasKey(ps => ps.IdPaymentStatus);
                paymentStatus.Property(ps => ps.Name).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<BudgetStatus>(budgetStatus =>
            {
                budgetStatus.HasKey(bs => bs.IdBudgetStatus);
                budgetStatus.Property(bs => bs.Name).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<Budget>(budget =>
            {
                budget.HasKey(b => b.IdBudget);
                budget.Property(b => b.IsCompleted).IsRequired();
                budget.Property(b => b.OpendDate).HasColumnType("date");
                budget.Property(b => b.BudgetYear).IsRequired().HasColumnType("date");
                budget.Property(b => b.Revenue).IsRequired().HasColumnType("money");
                budget.Property(b => b.Surplus).IsRequired().HasColumnType("money");
            });

            modelBuilder.Entity<Payment>(payment =>
            {
                payment.HasKey(p => p.IdPayment);
                payment.Property(p => p.Name).IsRequired().HasMaxLength(80);
                payment.Property(p => p.Charge).IsRequired().HasColumnType("money");
                payment.Property(p => p.Refund).IsRequired().HasColumnType("money");
                payment.Property(p => p.PaymentDate).HasColumnType("date");
                payment.Property(p => p.Message).HasMaxLength(350);
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<GeneralCategory> GeneralCategories { get; set; }
        public DbSet<DetailedCategory> DetailedCategories { get; set; }
        public DbSet<PaymentStatus> PaymentStatuses { get; set; }
        public DbSet<BudgetStatus> BudgetStatuses { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
