using Pinionszek_API.Models.DatabaseModel;

namespace Pinionszek_API.Services.DatabaseServices.PaymentService
{
    public interface IPaymentApiService
    {
        public Task<IEnumerable<Payment>> GetPaymentsAsync(int idBudget);
        public Task<IEnumerable<Payment>> GetAssignedPaymentsAsync(int friendTag);
        public Task<SharedPayment?> GetSharedPaymentDataAsync(int idPayment);
        public Task<Payment?> GetPaymentAsync(int idPayment, int idUser);
    }
}
