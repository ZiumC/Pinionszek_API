﻿using Microsoft.EntityFrameworkCore;
using Pinionszek_API.DbContexts;
using Pinionszek_API.Models.DatabaseModel;
using System;
using System.Linq;

namespace Pinionszek_API.Services.DatabaseServices.BudgetService
{
    public class BudgetApiService : IBudgetApiService
    {
        private readonly ProdDbContext _dbContext;
        public BudgetApiService(ProdDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Budget?> GetBudgetDataAsync(int idUser, DateTime budgetYear)
        {
            int month = budgetYear.Month;
            int year = budgetYear.Year;

            var budget =
                await (from b in _dbContext.Budgets
                       where b.IdUser == idUser &&
                            b.BudgetYear.Month == month &&
                            b.BudgetYear.Year == year

                       join bs in _dbContext.BudgetStatuses
                       on b.IdBudgetStatus equals bs.IdBudgetStatus

                       select new Budget
                       {
                           IdBudget = b.IdBudget,
                           IsCompleted = b.IsCompleted,
                           OpendDate = b.OpendDate,
                           Revenue = b.Revenue,
                           Surplus = b.Surplus,
                           BudgetYear = b.BudgetYear,
                           BudgetStatus = bs,
                       }).FirstOrDefaultAsync();

            return budget;
        }

        public async Task<IEnumerable<Payment>> GetPaymentsAsync(int idBudget)
        {
            return await
                (from p in _dbContext.Payments
                 where p.IdBudget == idBudget

                 join ps in _dbContext.PaymentStatuses
                 on p.IdPaymentStatus equals ps.IdPaymentStatus

                 join dc in _dbContext.DetailedCategories
                 on p.IdDetailedCategory equals dc.IdDetailedCategory

                 join gc in _dbContext.GeneralCategories
                 on dc.IdGeneralCategory equals gc.IdGeneralCategory

                 select new Payment
                 {
                     IdPayment = p.IdPayment,
                     Name = p.Name,
                     Charge = p.Charge,
                     Refund = p.Refund,
                     Message = p.Message,
                     PaymentDate = p.PaymentDate,
                     PaidOn = p.PaidOn,
                     PaymentAddedOn = p.PaymentAddedOn,
                     IdPaymentStatus = p.IdPaymentStatus,
                     PaymentStatus = p.PaymentStatus,
                     IdDetailedCategory = p.IdDetailedCategory,
                     SharedPayment = (from sp in _dbContext.SharedPayments
                                      where sp.IdPayment == p.IdPayment
                                      select sp).FirstOrDefault(),
                     DetailedCategory = new DetailedCategory
                     {
                         IdDetailedCategory = dc.IdDetailedCategory,
                         IdGeneralCategory = dc.IdGeneralCategory,
                         Name = dc.Name,
                         GeneralCategory = new GeneralCategory
                         {
                             IdGeneralCategory = gc.IdGeneralCategory,
                             Name = gc.Name,
                             IsDefault = gc.IsDefault
                         }
                     }
                 }).ToListAsync();
        }

        public async Task<SharedPayment?> GetSharedPaymentDataAsync(int idPayment)
        {
            return await _dbContext.SharedPayments
                .Where(sp => sp.IdPayment == idPayment)
                .FirstOrDefaultAsync();

        }

        public async Task<(string?, int?)> GetFriendReceiveNameAndTagAsync(int idSharedPayment)
        {
            var friendReceiverQuery = await (from sp in _dbContext.SharedPayments
                                             where sp.IdSharedPayment == idSharedPayment

                                             join f in _dbContext.Friends
                                             on sp.IdFriend equals f.IdFriend

                                             join u in _dbContext.Users
                                             on f.FriendTag equals u.UserTag

                                             select new { Login = u.Login, UserTag = u.UserTag }
                                     ).FirstOrDefaultAsync();

            return (friendReceiverQuery?.Login, friendReceiverQuery?.UserTag);
        }

        public async Task<(string?, int?)> GetFriendSenderNameAndTagAsync(int idSharedPayment)
        {
            var friendSenderQuery = await (from sp in _dbContext.SharedPayments
                                           where sp.IdSharedPayment == idSharedPayment

                                           join f in _dbContext.Friends
                                           on sp.IdFriend equals f.IdFriend

                                           join u in _dbContext.Users
                                           on f.IdUser equals u.IdUser

                                           select new { Login = u.Login, UserTag = u.UserTag }
                                           ).FirstOrDefaultAsync();

            return (friendSenderQuery?.Login, friendSenderQuery?.UserTag);
        }

        public async Task<IEnumerable<Payment>> GetAssignedPaymentsAsync(int friendTag)
        {
            return await (from sh in _dbContext.SharedPayments
                          join f in _dbContext.Friends
                          on sh.IdFriend equals f.IdFriend

                          where f.FriendTag == friendTag

                          join p in _dbContext.Payments
                          on sh.IdPayment equals p.IdPayment

                          join ps in _dbContext.PaymentStatuses
                          on p.IdPaymentStatus equals ps.IdPaymentStatus

                          join dc in _dbContext.DetailedCategories
                          on p.IdDetailedCategory equals dc.IdDetailedCategory

                          join gc in _dbContext.GeneralCategories
                          on dc.IdGeneralCategory equals gc.IdGeneralCategory

                          select new Payment
                          {
                              IdPayment = p.IdPayment,
                              Name = p.Name,
                              Charge = p.Charge,
                              Refund = p.Refund,
                              Message = p.Message,
                              PaymentDate = p.PaymentDate,
                              PaidOn = p.PaidOn,
                              PaymentAddedOn = p.PaymentAddedOn,
                              IdPaymentStatus = p.IdPaymentStatus,
                              PaymentStatus = p.PaymentStatus,
                              IdDetailedCategory = p.IdDetailedCategory,
                              SharedPayment = (from sp in _dbContext.SharedPayments
                                               where sp.IdPayment == p.IdPayment
                                               select sp).FirstOrDefault(),
                              DetailedCategory = new DetailedCategory
                              {
                                  IdDetailedCategory = dc.IdDetailedCategory,
                                  IdGeneralCategory = dc.IdGeneralCategory,
                                  Name = dc.Name,
                                  GeneralCategory = new GeneralCategory
                                  {
                                      IdGeneralCategory = gc.IdGeneralCategory,
                                      Name = gc.Name,
                                      IsDefault = gc.IsDefault
                                  }
                              }
                          }).ToListAsync();
        }

        public async Task<UserSettings?> GetUserSettingsAsync(int idUser)
        {
            return await _dbContext.UserSettings
                .Where(us => us.IdUser == idUser)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Budget>> GetBudgetsAsync(int idUser)
        {
            return await _dbContext.Budgets
                .Where(b => b.IdUser == idUser)
                .Join(_dbContext.BudgetStatuses,
                b => b.IdBudgetStatus,
                bs => bs.IdBudgetStatus,
                (b, bs) => new Budget
                {
                    IdBudget = b.IdBudget,
                    IsCompleted = b.IsCompleted,
                    OpendDate = b.OpendDate,
                    Revenue = b.Revenue,
                    Surplus = b.Surplus,
                    BudgetYear = b.BudgetYear,
                    IdBudgetStatus = b.IdBudgetStatus,
                    BudgetStatus = bs,
                    IdUser = b.IdUser
                }).ToListAsync();
        }

        public async Task<Payment?> GetPaymentAsync(int idPayment, int idUser)
        {
            var paymentUserQuery = await _dbContext.Budgets
                .Where(b => b.IdUser == idUser)
                .Join(_dbContext.Payments,
                b => b.IdBudget,
                p => p.IdBudget,
                (b, p) => p)
                .Where(p => p.IdPayment == idPayment)
                .Join(_dbContext.PaymentStatuses,
                p => p.IdPaymentStatus,
                ps => ps.IdPaymentStatus,
                (p, ps) => new Payment
                {
                    IdPayment = p.IdPayment,
                    Name = p.Name,
                    Charge = p.Charge,
                    Refund = p.Refund,
                    Message = p.Message,
                    PaymentDate = p.PaymentDate,
                    PaidOn = p.PaidOn,
                    PaymentAddedOn = p.PaymentAddedOn,
                    IdDetailedCategory = p.IdDetailedCategory,
                    PaymentStatus = new PaymentStatus
                    {
                        IdPaymentStatus = ps.IdPaymentStatus,
                        Name = ps.Name,
                    }
                }).FirstOrDefaultAsync();

            if (paymentUserQuery != null)
            {
                var detailedCategoryQuery = await _dbContext.DetailedCategories
                    .Where(dc => dc.IdDetailedCategory == paymentUserQuery.IdDetailedCategory)
                    .Join(_dbContext.GeneralCategories,
                    dc => dc.IdGeneralCategory,
                    gc => gc.IdGeneralCategory,
                    (dc, gc) => new DetailedCategory
                    {
                        IdDetailedCategory = dc.IdDetailedCategory,
                        Name = dc.Name,
                        GeneralCategory = new GeneralCategory
                        {
                            IdGeneralCategory = gc.IdGeneralCategory,
                            Name = gc.Name,
                        }
                    }).FirstOrDefaultAsync();

                var sharedPaymentQuery = await _dbContext.SharedPayments
                    .Where(sp => sp.IdPayment == paymentUserQuery.IdPayment)
                    .FirstOrDefaultAsync();

                if (detailedCategoryQuery != null)
                {
                    paymentUserQuery.DetailedCategory = detailedCategoryQuery;
                }

                paymentUserQuery.SharedPayment = sharedPaymentQuery;

            }

            return paymentUserQuery;
        }

        public async Task<IEnumerable<DetailedCategory>?> GetUserCategoriesAsync(int idUser)
        {
            return await _dbContext.DetailedCategories
                .Where(dc => dc.IdUser == idUser)
                .Join(_dbContext.GeneralCategories,
                dc => dc.IdGeneralCategory,
                gc => gc.IdGeneralCategory,
                (dc, gc) => new DetailedCategory
                {
                    IdDetailedCategory = dc.IdDetailedCategory,
                    Name = dc.Name,
                    GeneralCategory = new GeneralCategory
                    {
                        IdGeneralCategory = gc.IdGeneralCategory,
                        Name = gc.Name,
                        IsDefault = gc.IsDefault
                    }
                }).ToListAsync();
        }
    }
}
