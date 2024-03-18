using Microsoft.EntityFrameworkCore;
using Pinionszek_API.DbContexts;
using Pinionszek_API.Models.DatabaseModel;
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

        public async Task<(string?, int?)> GetFriendNameAndTagAsync(int idSharedPayment)
        {
            var friendQuery = await (from sp in _dbContext.SharedPayments
                                     where sp.IdSharedPayment == idSharedPayment

                                     join f in _dbContext.Friends
                                     on sp.IdFriend equals f.IdFriend

                                     join u in _dbContext.Users
                                     on f.FriendTag equals u.UserTag

                                     select new { Login = u.Login, UserTag = u.UserTag }).FirstOrDefaultAsync();

            return (friendQuery?.Login, friendQuery?.UserTag);
        }
    }
}
