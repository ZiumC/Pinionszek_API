using Pinionszek_API.Models.DatabaseModel;

namespace Pinionszek_API.Services.DatabaseServices.BudgetService
{
    public interface IBudgetApiService
    {
        public Task<Budget?> BudgetDataAsync(DateTime budgetYear, int idUser);
    }
}
