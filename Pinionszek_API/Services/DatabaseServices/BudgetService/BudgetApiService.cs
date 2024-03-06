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

        public async Task<Budget?> BudgetDataAsync(DateTime budgetYear, int idUser)
        {
            int month = budgetYear.Month;
            int year = budgetYear.Year;

            return await
                _dbContext.Budgets
                    .Where(
                            b => b.IdUser == idUser &&
                            b.BudgetYear.Month == month &&
                            b.BudgetYear.Year == year
                    ).FirstOrDefaultAsync();
        }
    }
}
