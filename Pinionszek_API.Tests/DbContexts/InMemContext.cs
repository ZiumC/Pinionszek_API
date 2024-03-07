using Microsoft.EntityFrameworkCore;
using Pinionszek_API.DbContexts;
using Pinionszek_API.Models.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinionszek_API.Tests.DbContexts
{
    public class InMemContext
    {
        public async Task<ProdDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ProdDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var prodDbContext = new ProdDbContext(options);
            prodDbContext.Database.EnsureCreated();

            if (prodDbContext.Users.Count() == 0)
            {
                prodDbContext.Users.Add(new User
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
                prodDbContext.Users.Add(new User
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
                prodDbContext.Users.Add(new User
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
                prodDbContext.Users.Add(new User
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

                await prodDbContext.SaveChangesAsync();
            }

            if (prodDbContext.UserSettings.Count() == 0)
            {
                prodDbContext.UserSettings.Add(new UserSettings
                {
                    IdUserSetting = 1,
                    UseBudgetRules = false,
                    DisplayBudgetRules = false,
                    IdUser = 2,
                    Needs = 0,
                    Savings = 0,
                    Wants = 0,
                });
                prodDbContext.UserSettings.Add(new UserSettings
                {
                    IdUserSetting = 2,
                    UseBudgetRules = true,
                    DisplayBudgetRules = false,
                    IdUser = 1,
                    Needs = new decimal(60.00),
                    Wants = new decimal(30.00),
                    Savings = new decimal(10.00),
                });
                prodDbContext.UserSettings.Add(new UserSettings
                {
                    IdUserSetting = 3,
                    UseBudgetRules = true,
                    DisplayBudgetRules = true,
                    IdUser = 4,
                    Needs = new decimal(70.00),
                    Wants = new decimal(25.00),
                    Savings = new decimal(5.00),
                });
                prodDbContext.UserSettings.Add(new UserSettings
                {
                    IdUserSetting = 4,
                    UseBudgetRules = true,
                    DisplayBudgetRules = true,
                    IdUser = 3,
                    Needs = new decimal(55.00),
                    Wants = new decimal(40.00),
                    Savings = new decimal(15.00),
                });

                await prodDbContext.SaveChangesAsync();
            }

            if (prodDbContext.Friends.Count() == 0)
            {
                prodDbContext.Friends.Add(new Friend
                {
                    IdFriend = 1,
                    FriendTag = 1002,
                    DateAdded = DateTime.Parse("2024-03-04"),
                    IdUser = 1
                });
                prodDbContext.Friends.Add(new Friend
                {
                    IdFriend = 2,
                    FriendTag = 1004,
                    DateAdded = DateTime.Parse("2024-03-04"),
                    IdUser = 1
                });
                prodDbContext.Friends.Add(new Friend
                {
                    IdFriend = 3,
                    FriendTag = 1001,
                    DateAdded = DateTime.Parse("2024-03-04"),
                    IdUser = 2
                });
                prodDbContext.Friends.Add(new Friend
                {
                    IdFriend = 4,
                    FriendTag = 1002,
                    DateAdded = DateTime.Parse("2023-11-01"),
                    IdUser = 3
                });
                prodDbContext.Friends.Add(new Friend
                {
                    IdFriend = 5,
                    FriendTag = 1003,
                    DateAdded = DateTime.Parse("2024-03-04"),
                    IdUser = 1
                });
                prodDbContext.Friends.Add(new Friend
                {
                    IdFriend = 6,
                    FriendTag = 1003,
                    DateAdded = DateTime.Parse("2023-11-01"),
                    IdUser = 2
                });

                await prodDbContext.SaveChangesAsync();
            }

            if (prodDbContext.GeneralCategories.Count() == 0)
            {
                prodDbContext.GeneralCategories.Add(new GeneralCategory
                {
                    IdGeneralCategory = 1,
                    Name = "Needs",
                    IsDefault = true
                });
                prodDbContext.GeneralCategories.Add(new GeneralCategory
                {
                    IdGeneralCategory = 2,
                    Name = "Wants",
                    IsDefault = true
                });
                prodDbContext.GeneralCategories.Add(new GeneralCategory
                {
                    IdGeneralCategory = 3,
                    Name = "Savings",
                    IsDefault = true
                });

                await prodDbContext.SaveChangesAsync();
            }

            if (prodDbContext.DetailedCategories.Count() == 0)
            {
                prodDbContext.DetailedCategories.Add(new DetailedCategory
                {
                    IdDetailedCategory = 1,
                    Name = "Rents",
                    IdGeneralCategory = 1,
                    IdUser = 1
                });
                prodDbContext.DetailedCategories.Add(new DetailedCategory
                {
                    IdDetailedCategory = 2,
                    Name = "Bills",
                    IdGeneralCategory = 1,
                    IdUser = 2
                });
                prodDbContext.DetailedCategories.Add(new DetailedCategory
                {
                    IdDetailedCategory = 3,
                    Name = "Health",
                    IdGeneralCategory = 1,
                    IdUser = 1
                });
                prodDbContext.DetailedCategories.Add(new DetailedCategory
                {
                    IdDetailedCategory = 4,
                    Name = "Fixed fee",
                    IdGeneralCategory = 1,
                    IdUser = 1
                });
                prodDbContext.DetailedCategories.Add(new DetailedCategory
                {
                    IdDetailedCategory = 5,
                    Name = "Pets",
                    IdGeneralCategory = 2,
                    IdUser = 1
                });
                prodDbContext.DetailedCategories.Add(new DetailedCategory
                {
                    IdDetailedCategory = 6,
                    Name = "Digital tools",
                    IdGeneralCategory = 2,
                    IdUser = 2
                });
                prodDbContext.DetailedCategories.Add(new DetailedCategory
                {
                    IdDetailedCategory = 7,
                    Name = "Phisical tools",
                    IdGeneralCategory = 2,
                    IdUser = 3
                });
                prodDbContext.DetailedCategories.Add(new DetailedCategory
                {
                    IdDetailedCategory = 8,
                    Name = "Food",
                    IdGeneralCategory = 2,
                    IdUser = 1
                });
                prodDbContext.DetailedCategories.Add(new DetailedCategory
                {
                    IdDetailedCategory = 9,
                    Name = "Future",
                    IdGeneralCategory = 3,
                    IdUser = 1
                });
                prodDbContext.DetailedCategories.Add(new DetailedCategory
                {
                    IdDetailedCategory = 10,
                    Name = "Future",
                    IdGeneralCategory = 3,
                    IdUser = 2
                });
                prodDbContext.DetailedCategories.Add(new DetailedCategory
                {
                    IdDetailedCategory = 11,
                    Name = "Future",
                    IdGeneralCategory = 3,
                    IdUser = 3
                });
                prodDbContext.DetailedCategories.Add(new DetailedCategory
                {
                    IdDetailedCategory = 12,
                    Name = "Food",
                    IdGeneralCategory = 2,
                    IdUser = 2
                });
                prodDbContext.DetailedCategories.Add(new DetailedCategory
                {
                    IdDetailedCategory = 13,
                    Name = "Food",
                    IdGeneralCategory = 2,
                    IdUser = 3
                });

                await prodDbContext.SaveChangesAsync();
            }

            if (prodDbContext.BudgetStatuses.Count() == 0)
            {
                prodDbContext.BudgetStatuses.Add(new BudgetStatus
                {
                    IdBudgetStatus = 1,
                    Name = "OPEND"
                });
                prodDbContext.BudgetStatuses.Add(new BudgetStatus
                {
                    IdBudgetStatus = 2,
                    Name = "COMPLETED"
                });
                prodDbContext.BudgetStatuses.Add(new BudgetStatus
                {
                    IdBudgetStatus = 3,
                    Name = "NOT OPEND YET"
                });

                await prodDbContext.SaveChangesAsync();
            }

            if (prodDbContext.Budgets.Count() == 0)
            {
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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

                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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
                prodDbContext.Budgets.Add(new Budget
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

                await prodDbContext.SaveChangesAsync();
            }

            if (prodDbContext.PaymentStatuses.Count() == 0)
            {
                prodDbContext.PaymentStatuses.Add(new PaymentStatus
                {
                    IdPaymentStatus = 1,
                    Name = "PAYED"
                });
                prodDbContext.PaymentStatuses.Add(new PaymentStatus
                {
                    IdPaymentStatus = 2,
                    Name = "NOT PAYED YET"
                });

                await prodDbContext.SaveChangesAsync();
            }

            if (prodDbContext.Payments.Count() == 0)
            {
                prodDbContext.Payments.Add(new Payment
                {
                    IdPayment = 1,
                    Name = "Fryzjer dla psa",
                    Charge = new decimal(180.00),
                    Refund = new decimal(75.00),
                    Message = "Fryzjer małego pieska domowego",
                    PaymentDate = DateTime.Parse("2024-01-29"),
                    PaidOn = null,
                    PaymentAddedOn = DateTime.Parse("2024-01-01"),
                    IdPaymentStatus = 2,
                    IdBudget = 1,
                    IdDetailedCategory = 5,
                });
                prodDbContext.Payments.Add(new Payment
                {
                    IdPayment = 2,
                    Name = "Mieszkanie",
                    Charge = new decimal(670.00),
                    Refund = new decimal(0.00),
                    Message = "",
                    PaymentDate = DateTime.Parse("2024-01-15"),
                    PaidOn = null,
                    PaymentAddedOn = DateTime.Parse("2024-01-01"),
                    IdPaymentStatus = 2,
                    IdBudget = 1,
                    IdDetailedCategory = 1,
                });
                prodDbContext.Payments.Add(new Payment
                {
                    IdPayment = 3,
                    Name = "Prywatne ubezpieczenie zdrowotne",
                    Charge = new decimal(570.00),
                    Refund = new decimal(0.00),
                    Message = "",
                    PaymentDate = null,
                    PaidOn = DateTime.Parse("2024-01-02"),
                    PaymentAddedOn = DateTime.Parse("2024-01-02"),
                    IdPaymentStatus = 1,
                    IdBudget = 1,
                    IdDetailedCategory = 3,
                });
                prodDbContext.Payments.Add(new Payment
                {
                    IdPayment = 4,
                    Name = "Platforma Netflix",
                    Charge = new decimal(35.99),
                    Refund = new decimal(11.00),
                    Message = "",
                    PaymentDate = DateTime.Parse("2024-01-12"),
                    PaidOn = null,
                    PaymentAddedOn = DateTime.Parse("2024-01-05"),
                    IdPaymentStatus = 2,
                    IdBudget = 1,
                    IdDetailedCategory = 4,
                });
                prodDbContext.Payments.Add(new Payment
                {
                    IdPayment = 12,
                    Name = "Biedronka",
                    Charge = new decimal(53.12),
                    Refund = new decimal(0.00),
                    Message = "",
                    PaymentDate = null,
                    PaidOn = DateTime.Parse("2024-01-06"),
                    PaymentAddedOn = DateTime.Parse("2024-01-06"),
                    IdPaymentStatus = 1,
                    IdBudget = 1,
                    IdDetailedCategory = 8,
                });
                prodDbContext.Payments.Add(new Payment
                {
                    IdPayment = 5,
                    Name = "Kaufland",
                    Charge = new decimal(11.23),
                    Refund = new decimal(0.00),
                    Message = "",
                    PaymentDate = null,
                    PaidOn = DateTime.Parse("2024-01-02"),
                    PaymentAddedOn = DateTime.Parse("2024-01-02"),
                    IdPaymentStatus = 1,
                    IdBudget = 1,
                    IdDetailedCategory = 8,
                });
                prodDbContext.Payments.Add(new Payment
                {
                    IdPayment = 6,
                    Name = "Savings",
                    Charge = new decimal(100.00),
                    Refund = new decimal(0.00),
                    Message = "",
                    PaymentDate = null,
                    PaidOn = DateTime.Parse("2024-01-01"),
                    PaymentAddedOn = DateTime.Parse("2024-01-01"),
                    IdPaymentStatus = 1,
                    IdBudget = 1,
                    IdDetailedCategory = 9,
                });
                prodDbContext.Payments.Add(new Payment
                {
                    IdPayment = 7,
                    Name = "Czynsz",
                    Charge = new decimal(861.00),
                    Refund = new decimal(0.00),
                    Message = "",
                    PaymentDate = DateTime.Parse("2024-01-20"),
                    PaidOn = null,
                    PaymentAddedOn = DateTime.Parse("2024-01-02"),
                    IdPaymentStatus = 2,
                    IdBudget = 13,
                    IdDetailedCategory = 2,
                });
                prodDbContext.Payments.Add(new Payment
                {
                    IdPayment = 13,
                    Name = "Opieka zdrowotna",
                    Charge = new decimal(305.00),
                    Refund = new decimal(0.00),
                    Message = "",
                    PaymentDate = DateTime.Parse("2024-01-20"),
                    PaidOn = null,
                    PaymentAddedOn = DateTime.Parse("2024-01-02"),
                    IdPaymentStatus = 2,
                    IdBudget = 13,
                    IdDetailedCategory = 2,
                });
                prodDbContext.Payments.Add(new Payment
                {
                    IdPayment = 8,
                    Name = "Oprogramowanie do montowania dysków",
                    Charge = new decimal(425.00),
                    Refund = new decimal(0.00),
                    Message = "",
                    PaymentDate = null,
                    PaidOn = DateTime.Parse("2024-01-10"),
                    PaymentAddedOn = DateTime.Parse("2024-01-10"),
                    IdPaymentStatus = 1,
                    IdBudget = 13,
                    IdDetailedCategory = 6,
                });
                prodDbContext.Payments.Add(new Payment
                {
                    IdPayment = 9,
                    Name = "Savings",
                    Charge = new decimal(200.00),
                    Refund = new decimal(0.00),
                    Message = "",
                    PaymentDate = null,
                    PaidOn = DateTime.Parse("2024-01-12"),
                    PaymentAddedOn = DateTime.Parse("2024-01-12"),
                    IdPaymentStatus = 1,
                    IdBudget = 13,
                    IdDetailedCategory = 10,
                });
                prodDbContext.Payments.Add(new Payment
                {
                    IdPayment = 10,
                    Name = "Lidl",
                    Charge = new decimal(99.12),
                    Refund = new decimal(40.00),
                    Message = "",
                    PaymentDate = null,
                    PaidOn = DateTime.Parse("2024-01-19"),
                    PaymentAddedOn = DateTime.Parse("2024-01-19"),
                    IdPaymentStatus = 1,
                    IdBudget = 13,
                    IdDetailedCategory = 12,
                });
                prodDbContext.Payments.Add(new Payment
                {
                    IdPayment = 11,
                    Name = "Biedronka",
                    Charge = new decimal(7.89),
                    Refund = new decimal(0.00),
                    Message = "Podroby dla piesków",
                    PaymentDate = null,
                    PaidOn = DateTime.Parse("2024-01-19"),
                    PaymentAddedOn = DateTime.Parse("2024-01-19"),
                    IdPaymentStatus = 1,
                    IdBudget = 13,
                    IdDetailedCategory = 12,
                });

                await prodDbContext.SaveChangesAsync();
            }

            if (prodDbContext.SharedPayments.Count() == 0)
            {
                prodDbContext.SharedPayments.Add(new SharedPayment
                {
                    IdSharedPayment = 1,
                    IdPayment = 1,
                    IdFriend = 1
                });
                prodDbContext.SharedPayments.Add(new SharedPayment
                {
                    IdSharedPayment = 2,
                    IdPayment = 4,
                    IdFriend = 1
                });
                prodDbContext.SharedPayments.Add(new SharedPayment
                {
                    IdSharedPayment = 3,
                    IdPayment = 10,
                    IdFriend = 3
                });

                await prodDbContext.SaveChangesAsync();
            }

            return prodDbContext;
        }
    }
}
