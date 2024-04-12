using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.PostDto;

namespace Pinionszek_API.Services.DatabaseServices.PaymentService
{
    public interface IPaymentApiService
    {
        public Task<IEnumerable<Payment>> GetPaymentsAsync(int idBudget);
        public Task<IEnumerable<Payment>> GetAssignedPaymentsAsync(int friendTag);
        public Task<SharedPayment?> GetSharedPaymentDataAsync(int idPayment);
        public Task<Payment?> GetPaymentAsync(int idPayment, int idUser);
        public Task<IEnumerable<GeneralCategory>> GetDefaultGeneralCategoriesAsync();
        public Task<bool> CreatePayment(Payment payment, int idBudget, int friendTag);
        //public Task<PaymentStatus?> GetPaymentStatusAsync(PaymentStatusEnum status);
    }
}
