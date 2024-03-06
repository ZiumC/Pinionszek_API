using Microsoft.EntityFrameworkCore;
using Pinionszek_API.DbContexts;
using Pinionszek_API.Models.DatabaseModel;

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
                            SharedPayments = (from s in _dbContext.SharedPayments
                                             where s.IdPayment == p.IdPayment
                                             select s).ToList(),
                        }
                    ).ToListAsync();

                budget.Payments = payments;
            }

            return budget;
        }
    }
}
