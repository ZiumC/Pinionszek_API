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

        public async Task<Budget?> GetBudgetAsync(int idUser, DateTime budgetYear)
        {
            int month = budgetYear.Month;
            int year = budgetYear.Year;

            var budget = await
                _dbContext.Budgets
                    .Where(
                            b => b.IdUser == idUser &&
                            b.BudgetYear.Month == month &&
                            b.BudgetYear.Year == year
                    ).FirstOrDefaultAsync();

            if (budget != null)
            {
                var payments = await
                    (
                        from p in _dbContext.Payments
                        where p.IdBudget == budget.IdBudget

                        join ps in _dbContext.PaymentStatuses
                        on p.IdPaymentStatus equals ps.IdPaymentStatus

                        join dc in _dbContext.DetailedCategories
                        on p.IdDetailedCategory equals dc.IdDetailedCategory

                        join gc in _dbContext.GeneralCategories
                        on dc.IdGeneralCategory equals gc.IdGeneralCategory

                        join sp in _dbContext.SharedPayments
                        on p.IdPayment equals sp.IdPayment

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
                            PaymentStatus = ps,
                            DetailedCategory = new DetailedCategory
                            {
                                Name = dc.Name,
                                GeneralCategory = gc,
                            },
                            SharedPayment = sp
                        }
                    ).ToListAsync();

                budget.Payments = payments;
            }

            return budget;
        }

        public async Task<(string?, int?)> GetFriendNameAndTagAsync(int idSharedPayment)
        {
            var friendQuery = await (from sp in _dbContext.SharedPayments
                                     where sp.IdSharedPayment == idSharedPayment

                                     join f in _dbContext.Friends
                                     on sp.IdFriend equals f.IdFriend

                                     join u in _dbContext.Users
                                     on f.FriendTag equals u.UserTag

                                     select new { Login = u.Login, FriendTag = f.FriendTag}).FirstOrDefaultAsync();

            return (friendQuery?.Login, friendQuery?.FriendTag);
        }
    }
}
