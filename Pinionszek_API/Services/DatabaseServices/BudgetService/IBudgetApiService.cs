using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDto;

namespace Pinionszek_API.Services.DatabaseServices.BudgetService
{
    public interface IBudgetApiService
    {
        public Task<Budget?> GetBudgetDataAsync(int idUser, DateTime budgetYear);
        public Task<IEnumerable<Payment>> GetPaymentsAsync(int idBudget);
        public Task<SharedPayment?> GetSharedPaymentDataAsync(int idPayment);
        public Task<(string?, int?)> GetFriendReceiveNameAndTagAsync(int idSharedPayment);
        public Task<(string?, int?)> GetFriendSenderNameAndTagAsync(int idSharedPayment);
        public Task<IEnumerable<Payment>> GetAssignedPaymentsAsync(int friendTag);
        public Task<UserSettings?> GetUserSettingsAsync(int idUser);
        public Task<IEnumerable<Budget>> GetBudgetsAsync(int idUser);
        public Task<Payment?> GetPaymentAsync(int idPayment, int idUser);
        public Task<IEnumerable<GeneralCategory>> GetDefaultGeneralCategoriesAsync();
    }
}
