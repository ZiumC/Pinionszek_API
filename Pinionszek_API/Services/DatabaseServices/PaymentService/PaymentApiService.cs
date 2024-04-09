using Microsoft.EntityFrameworkCore;
using Pinionszek_API.DbContexts;
using Pinionszek_API.Models.DatabaseModel;

namespace Pinionszek_API.Services.DatabaseServices.PaymentService
{
    public class PaymentApiService : IPaymentApiService
    {
        private readonly ProdDbContext _dbContext;
        public PaymentApiService(ProdDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Payment>> GetPaymentsAsync(int idBudget)
        {
            return await
                (from p in _dbContext.Payments
                 where p.IdBudget == idBudget

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
                     IdPaymentStatus = p.IdPaymentStatus,
                     PaymentStatus = p.PaymentStatus,
                     IdDetailedCategory = p.IdDetailedCategory,
                     SharedPayment = (from sp in _dbContext.SharedPayments
                                      where sp.IdPayment == p.IdPayment
                                      select sp).FirstOrDefault(),
                     DetailedCategory = new DetailedCategory
                     {
                         IdDetailedCategory = dc.IdDetailedCategory,
                         IdGeneralCategory = dc.IdGeneralCategory,
                         Name = dc.Name,
                         GeneralCategory = new GeneralCategory
                         {
                             IdGeneralCategory = gc.IdGeneralCategory,
                             Name = gc.Name,
                             IsDefault = gc.IsDefault
                         }
                     }
                 }).ToListAsync();
        }
    }
}
