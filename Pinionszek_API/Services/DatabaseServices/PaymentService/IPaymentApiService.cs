using Pinionszek_API.Models.DatabaseModel;

namespace Pinionszek_API.Services.DatabaseServices.PaymentService
{
    public interface IPaymentApiService
    {
        public Task<IEnumerable<Payment>> GetPaymentsAsync(int idBudget);
    }
}
