using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDto;

namespace Pinionszek_API.Services.DatabaseServices.BudgetService
{
    public interface IBudgetApiService
    {
        public Task<Budget?> GetBudgetDataAsync(int idUser, DateTime budgetDate);
        public Task<Budget?> GetBudgetDataAsync(int idUser, int idBudget);
        public Task<(string?, int?)> GetFriendReceiveNameAndTagAsync(int idSharedPayment);
        public Task<(string?, int?)> GetFriendSenderNameAndTagAsync(int idSharedPayment);
        public Task<IEnumerable<Budget>> GetBudgetsAsync(int idUser);
    }
}
