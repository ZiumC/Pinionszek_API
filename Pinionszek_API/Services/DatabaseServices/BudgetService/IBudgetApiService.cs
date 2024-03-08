using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDTO;

namespace Pinionszek_API.Services.DatabaseServices.BudgetService
{
    public interface IBudgetApiService
    {
        public Task<Budget?> GetBudgetAsync(int idUser, DateTime budgetYear);
        public Task<string?> GetFriendNameAsync(int idSharedPayment);
    }
}
