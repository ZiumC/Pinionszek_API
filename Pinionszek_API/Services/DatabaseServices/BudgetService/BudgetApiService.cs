using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<GeneralCategory>> GetDefaultGeneralCategoriesAsync()
        {
            return await _dbContext.GeneralCategories
                .Where(gc => gc.IsDefault)
                .ToListAsync();
        }
    }
}
