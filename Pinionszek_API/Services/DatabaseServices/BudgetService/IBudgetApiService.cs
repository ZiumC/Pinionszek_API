using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDTO;

namespace Pinionszek_API.Services.DatabaseServices.BudgetService
{
    public interface IBudgetApiService
    {
        public Task<Budget?> GetBudgetDataAsync(int idUser, DateTime budgetYear);
        public Task<IEnumerable<Payment>> GetPaymentsAsync(int idBudget);
        public Task<SharedPayment?> GetSharedPaymentDataAsync(int idPayment);
        public Task<(string?, int?)> GetFriendNameAndTagAsync(int idSharedPayment);
    }
}
