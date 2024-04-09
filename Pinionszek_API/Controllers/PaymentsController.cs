using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pinionszek_API.Models.DTOs.GetDto.Payments;
using Pinionszek_API.Services.DatabaseServices.BudgetService;
using Pinionszek_API.Services.DatabaseServices.PaymentService;
using System.ComponentModel.DataAnnotations;

namespace Pinionszek_API.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPaymentApiService _paymentService;
        private readonly IBudgetApiService _budgetService;

        public PaymentsController(IPaymentApiService paymentService, IBudgetApiService budgetService, 
            IMapper mapper)
        {
            _mapper = mapper;
            _paymentService = paymentService;
            _budgetService = budgetService;
        }

        /// <summary>
        /// Get upcoming private payments by user ID and budget date 
        /// </summary>
        /// <param name="idUser">User ID</param>
        /// <param name="date">Budget date</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        [HttpGet("upcoming-payments/private")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetPrivatePaymentDto>))]
        public async Task<IActionResult> GetUpcomingPrivatePaymentsAsync
            ([Required] DateTime date, [Required] int idUser, int page = 1, int pageSize = 20)
        {
            if (idUser <= 0)
            {
                ModelState.AddModelError("error", "User ID is invalid");
                return BadRequest(ModelState);
            }

            if (date == DateTime.MinValue)
            {
                ModelState.AddModelError("error", "Budget date is not specified");
                return BadRequest(ModelState);
            }

            if (page <= 0)
            {
                ModelState.AddModelError("error", "Page number is invalid");
                return BadRequest(ModelState);
            }

            if (pageSize <= 0)
            {
                ModelState.AddModelError("error", "Page size is invalid");
                return BadRequest(ModelState);
            }

            var budgetData = await _budgetService
                .GetBudgetDataAsync(idUser, date);
            if (budgetData == null)
            {
                return NotFound();
            }

            var budgetPaymentsData = await _paymentService
                .GetPaymentsAsync(budgetData.IdBudget);
            if (budgetPaymentsData == null || budgetPaymentsData.Count() == 0)
            {
                return NotFound();
            }

            var upcomingPaymentsData = budgetPaymentsData
                .Where(bpd => bpd.PaymentDate != null)
                .ToList();
            if (upcomingPaymentsData == null || upcomingPaymentsData.Count() == 0)
            {
                return NotFound();
            }

            foreach (var payment in upcomingPaymentsData)
            {
                var sharedPaymentData = await _budgetService
                    .GetSharedPaymentDataAsync(payment.IdPayment);

                payment.SharedPayment = sharedPaymentData;
            }

            var upcomingPrivatePaymentsData = upcomingPaymentsData
                .Where(upd => upd.SharedPayment == null || upd.SharedPayment?.IdSharedPayment == 0)
                .OrderBy(upd => upd.PaymentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            if (upcomingPrivatePaymentsData == null || upcomingPrivatePaymentsData.Count() == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<GetPrivatePaymentDto>>(upcomingPrivatePaymentsData));
        }
    }
}
