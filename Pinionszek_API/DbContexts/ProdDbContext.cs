using Microsoft.EntityFrameworkCore;
using Pinionszek_API.Models.DatabaseModel;
using System;

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

            modelBuilder.Entity<SharedPayment>(sharedPayment =>
            {
                sharedPayment.HasKey(sp => sp.IdSharedPayment);
            });

            modelBuilder.Entity<User>(user =>
            {
                user.HasData(new User
                {
                    IdUser = 1,
                    UserTag = 1001,
                    Email = "test1@test.pl",
                    Login = "test1",
                    Password = "password1",
                    PasswordSalt = "passsalt",
                    RegisteredAt = DateTime.Parse("2024-03-04"),
                    RefreshToken = null,
                    LoginAttempts = 0,
                    BlockedTo = null
                });
                user.HasData(new User
                {
                    IdUser = 2,
                    UserTag = 1002,
                    Email = "test2@test.pl",
                    Login = "test2",
                    Password = "password2",
                    PasswordSalt = "passsalt",
                    RegisteredAt = DateTime.Parse("2023-02-09"),
                    RefreshToken = null,
                    LoginAttempts = 0,
                    BlockedTo = null
                });
                user.HasData(new User
                {
                    IdUser = 3,
                    UserTag = 1003,
                    Email = "test3@test.pl",
                    Login = "test3",
                    Password = "password3",
                    PasswordSalt = "passsalt",
                    RegisteredAt = DateTime.Parse("2023-11-01"),
                    RefreshToken = null,
                    LoginAttempts = 0,
                    BlockedTo = null
                });
                user.HasData(new User
                {
                    IdUser = 4,
                    UserTag = 1004,
                    Email = "test4@test.pl",
                    Login = "test4",
                    Password = "password4",
                    PasswordSalt = "passsalt",
                    RegisteredAt = DateTime.Parse("2023-12-12"),
                    RefreshToken = null,
                    LoginAttempts = 0,
                    BlockedTo = null
                });
            });

            modelBuilder.Entity<UserSettings>(userSettings =>
            {
                userSettings.HasData(new UserSettings
                {
                    IdUserSetting = 1,
                    UseBudgetRules = false,
                    DisplayBudgetRules = false,
                    IdUser = 2,
                    Needs = 0,
                    Savings = 0,
                    Wants = 0,
                });
                userSettings.HasData(new UserSettings
                {
                    IdUserSetting = 2,
                    UseBudgetRules = true,
                    DisplayBudgetRules = false,
                    IdUser = 1,
                    Needs = new decimal(60.00),
                    Wants = new decimal(30.00),
                    Savings = new decimal(10.00),
                });
                userSettings.HasData(new UserSettings
                {
                    IdUserSetting = 3,
                    UseBudgetRules = true,
                    DisplayBudgetRules = true,
                    IdUser = 4,
                    Needs = new decimal(70.00),
                    Wants = new decimal(25.00),
                    Savings = new decimal(5.00),
                });
                userSettings.HasData(new UserSettings
                {
                    IdUserSetting = 4,
                    UseBudgetRules = true,
                    DisplayBudgetRules = true,
                    IdUser = 3,
                    Needs = new decimal(55.00),
                    Wants = new decimal(40.00),
                    Savings = new decimal(15.00),
                });
            });

            modelBuilder.Entity<Friend>(friend =>
            {
                friend.HasData(new Friend
                {
                    IdFriend = 1,
                    FriendTag = 1002,
                    DateAdded = DateTime.Parse("2024-03-04"),
                    IdUser = 1
                });
                friend.HasData(new Friend
                {
                    IdFriend = 2,
                    FriendTag = 1004,
                    DateAdded = DateTime.Parse("2024-03-04"),
                    IdUser = 1
                });
                friend.HasData(new Friend
                {
                    IdFriend = 3,
                    FriendTag = 1001,
                    DateAdded = DateTime.Parse("2024-03-04"),
                    IdUser = 2
                });
                friend.HasData(new Friend
                {
                    IdFriend = 4,
                    FriendTag = 1002,
                    DateAdded = DateTime.Parse("2023-11-01"),
                    IdUser = 3
                });
                friend.HasData(new Friend
                {
                    IdFriend = 5,
                    FriendTag = 1004,
                    DateAdded = DateTime.Parse("2024-03-04"),
                    IdUser = 1
                });
                friend.HasData(new Friend
                {
                    IdFriend = 6,
                    FriendTag = 1003,
                    DateAdded = DateTime.Parse("2023-11-01"),
                    IdUser = 2
                });
            });

            modelBuilder.Entity<GeneralCategory>(generalCategory =>
            {
                generalCategory.HasData(new GeneralCategory
                {
                    IdGeneralCategory = 1,
                    Name = "Needs",
                    IsDefault = true
                });
                generalCategory.HasData(new GeneralCategory
                {
                    IdGeneralCategory = 2,
                    Name = "Wants",
                    IsDefault = true
                });
                generalCategory.HasData(new GeneralCategory
                {
                    IdGeneralCategory = 3,
                    Name = "Savings",
                    IsDefault = true
                });
            });

            modelBuilder.Entity<DetailedCategory>(detailedCategory =>
            {
                detailedCategory.HasData(new DetailedCategory
                {
                    IdDetailedCategory = 1,
                    Name = "Rents",
                    IdGeneralCategory = 1,
                    IdUser = 1
                });
                detailedCategory.HasData(new DetailedCategory
                {
                    IdDetailedCategory = 2,
                    Name = "Bills",
                    IdGeneralCategory = 1,
                    IdUser = 2
                });
                detailedCategory.HasData(new DetailedCategory
                {
                    IdDetailedCategory = 3,
                    Name = "Health",
                    IdGeneralCategory = 1,
                    IdUser = 1
                });
                detailedCategory.HasData(new DetailedCategory
                {
                    IdDetailedCategory = 4,
                    Name = "Fixed fee",
                    IdGeneralCategory = 1,
                    IdUser = 1
                });
                detailedCategory.HasData(new DetailedCategory
                {
                    IdDetailedCategory = 5,
                    Name = "Pets",
                    IdGeneralCategory = 2,
                    IdUser = 1
                });
                detailedCategory.HasData(new DetailedCategory
                {
                    IdDetailedCategory = 6,
                    Name = "Digital tools",
                    IdGeneralCategory = 2,
                    IdUser = 2
                });
                detailedCategory.HasData(new DetailedCategory
                {
                    IdDetailedCategory = 7,
                    Name = "Phisical tools",
                    IdGeneralCategory = 2,
                    IdUser = 3
                });
                detailedCategory.HasData(new DetailedCategory
                {
                    IdDetailedCategory = 8,
                    Name = "Food",
                    IdGeneralCategory = 2,
                    IdUser = 1
                });
                detailedCategory.HasData(new DetailedCategory
                {
                    IdDetailedCategory = 9,
                    Name = "Future",
                    IdGeneralCategory = 3,
                    IdUser = 1
                });
                detailedCategory.HasData(new DetailedCategory
                {
                    IdDetailedCategory = 10,
                    Name = "Future",
                    IdGeneralCategory = 3,
                    IdUser = 2
                });
                detailedCategory.HasData(new DetailedCategory
                {
                    IdDetailedCategory = 11,
                    Name = "Future",
                    IdGeneralCategory = 3,
                    IdUser = 3
                });
                detailedCategory.HasData(new DetailedCategory
                {
                    IdDetailedCategory = 12,
                    Name = "Food",
                    IdGeneralCategory = 2,
                    IdUser = 2
                });
                detailedCategory.HasData(new DetailedCategory
                {
                    IdDetailedCategory = 13,
                    Name = "Food",
                    IdGeneralCategory = 2,
                    IdUser = 3
                });
            });

            modelBuilder.Entity<BudgetStatus>(budgetstatus =>
            {
                budgetstatus.HasData(new BudgetStatus
                {
                    IdBudgetStatus = 1,
                    Name = "OPEND"
                });
                budgetstatus.HasData(new BudgetStatus
                {
                    IdBudgetStatus = 2,
                    Name = "COMPLETED"
                });
                budgetstatus.HasData(new BudgetStatus
                {
                    IdBudgetStatus = 3,
                    Name = "NOT OPEND YET"
                });
            });

            modelBuilder.Entity<Budget>(budget =>
            {
                budget.HasData(new Budget
                {
                    IdBudget = 1,
                    IsCompleted = false,
                    OpendDate = DateTime.Parse("2024-01-01"),
                    Revenue = new decimal(2213.00),
                    Surplus = new decimal(12.00),
                    BudgetYear = DateTime.Parse("2024-01-01"),
                    IdBudgetStatus = 1,
                    IdUser = 1
                });
                budget.HasData(new Budget
                {
                    IdBudget = 2,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-02-01"),
                    IdBudgetStatus = 3,
                    IdUser = 1
                });
                budget.HasData(new Budget
                {
                    IdBudget = 3,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-03-01"),
                    IdBudgetStatus = 3,
                    IdUser = 1
                });
                budget.HasData(new Budget
                {
                    IdBudget = 4,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-04-01"),
                    IdBudgetStatus = 3,
                    IdUser = 1
                });
                budget.HasData(new Budget
                {
                    IdBudget = 5,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-05-01"),
                    IdBudgetStatus = 3,
                    IdUser = 1
                });
                budget.HasData(new Budget
                {
                    IdBudget = 6,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-06-01"),
                    IdBudgetStatus = 3,
                    IdUser = 1
                });
                budget.HasData(new Budget
                {
                    IdBudget = 7,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-07-01"),
                    IdBudgetStatus = 3,
                    IdUser = 1
                });
                budget.HasData(new Budget
                {
                    IdBudget = 8,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-08-01"),
                    IdBudgetStatus = 3,
                    IdUser = 1
                });
                budget.HasData(new Budget
                {
                    IdBudget = 9,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-09-01"),
                    IdBudgetStatus = 3,
                    IdUser = 1
                });
                budget.HasData(new Budget
                {
                    IdBudget = 10,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-10-01"),
                    IdBudgetStatus = 3,
                    IdUser = 1
                });
                budget.HasData(new Budget
                {
                    IdBudget = 11,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-11-01"),
                    IdBudgetStatus = 3,
                    IdUser = 1
                });
                budget.HasData(new Budget
                {
                    IdBudget = 12,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-12-01"),
                    IdBudgetStatus = 3,
                    IdUser = 1
                });
                budget.HasData(new Budget
                {
                    IdBudget = 13,
                    IsCompleted = false,
                    OpendDate = DateTime.Parse("2023-12-29"),
                    Revenue = new decimal(3250.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-01-01"),
                    IdBudgetStatus = 1,
                    IdUser = 2
                });
                budget.HasData(new Budget
                {
                    IdBudget = 14,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-02-01"),
                    IdBudgetStatus = 3,
                    IdUser = 2
                });
                budget.HasData(new Budget
                {
                    IdBudget = 15,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-03-01"),
                    IdBudgetStatus = 3,
                    IdUser = 2
                });
                budget.HasData(new Budget
                {
                    IdBudget = 16,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-04-01"),
                    IdBudgetStatus = 3,
                    IdUser = 2
                });
                budget.HasData(new Budget
                {
                    IdBudget = 17,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-05-01"),
                    IdBudgetStatus = 3,
                    IdUser = 2
                });
                budget.HasData(new Budget
                {
                    IdBudget = 18,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-06-01"),
                    IdBudgetStatus = 3,
                    IdUser = 2
                });
                budget.HasData(new Budget
                {
                    IdBudget = 19,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-07-01"),
                    IdBudgetStatus = 3,
                    IdUser = 2
                });
                budget.HasData(new Budget
                {
                    IdBudget = 20,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-08-01"),
                    IdBudgetStatus = 3,
                    IdUser = 2
                });
                budget.HasData(new Budget
                {
                    IdBudget = 21,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-09-01"),
                    IdBudgetStatus = 3,
                    IdUser = 2
                });
                budget.HasData(new Budget
                {
                    IdBudget = 22,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-10-01"),
                    IdBudgetStatus = 3,
                    IdUser = 2
                });
                budget.HasData(new Budget
                {
                    IdBudget = 23,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-11-01"),
                    IdBudgetStatus = 3,
                    IdUser = 2
                });
                budget.HasData(new Budget
                {
                    IdBudget = 24,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-12-01"),
                    IdBudgetStatus = 3,
                    IdUser = 2
                });

                budget.HasData(new Budget
                {
                    IdBudget = 25,
                    IsCompleted = false,
                    OpendDate = DateTime.Parse("2024-01-11"),
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(120.00),
                    BudgetYear = DateTime.Parse("2024-01-01"),
                    IdBudgetStatus = 1,
                    IdUser = 3
                });
                budget.HasData(new Budget
                {
                    IdBudget = 26,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-02-01"),
                    IdBudgetStatus = 3,
                    IdUser = 3
                });
                budget.HasData(new Budget
                {
                    IdBudget = 27,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-03-01"),
                    IdBudgetStatus = 3,
                    IdUser = 3
                });
                budget.HasData(new Budget
                {
                    IdBudget = 28,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-04-01"),
                    IdBudgetStatus = 3,
                    IdUser = 3
                });
                budget.HasData(new Budget
                {
                    IdBudget = 29,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-05-01"),
                    IdBudgetStatus = 3,
                    IdUser = 3
                });
                budget.HasData(new Budget
                {
                    IdBudget = 30,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-06-01"),
                    IdBudgetStatus = 3,
                    IdUser = 3
                });
                budget.HasData(new Budget
                {
                    IdBudget = 31,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-07-01"),
                    IdBudgetStatus = 3,
                    IdUser = 3
                });
                budget.HasData(new Budget
                {
                    IdBudget = 32,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-08-01"),
                    IdBudgetStatus = 3,
                    IdUser = 3
                });
                budget.HasData(new Budget
                {
                    IdBudget = 33,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-09-01"),
                    IdBudgetStatus = 3,
                    IdUser = 3
                });
                budget.HasData(new Budget
                {
                    IdBudget = 34,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-10-01"),
                    IdBudgetStatus = 3,
                    IdUser = 3
                });
                budget.HasData(new Budget
                {
                    IdBudget = 35,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-11-01"),
                    IdBudgetStatus = 3,
                    IdUser = 3
                });
                budget.HasData(new Budget
                {
                    IdBudget = 36,
                    IsCompleted = false,
                    OpendDate = null,
                    Revenue = new decimal(0.00),
                    Surplus = new decimal(0.00),
                    BudgetYear = DateTime.Parse("2024-12-01"),
                    IdBudgetStatus = 3,
                    IdUser = 3
                });
            });

            modelBuilder.Entity<PaymentStatus>(paymentStatus =>
            {
                paymentStatus.HasData(new PaymentStatus
                {
                    IdPaymentStatus = 1,
                    Name = "PAYED"
                });
                paymentStatus.HasData(new PaymentStatus
                {
                    IdPaymentStatus = 2,
                    Name = "NOT PAYED YET"
                });
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
        public DbSet<SharedPayment> SharedPayments { get; set; }
    }
}
