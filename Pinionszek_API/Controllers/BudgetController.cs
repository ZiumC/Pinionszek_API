using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pinionszek_API.DbContexts;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDTO;
using Pinionszek_API.Services.DatabaseServices.BudgetService;

namespace Pinionszek_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetApiService _budgetService;
        private readonly IMapper _mapper;

        public BudgetController(IBudgetApiService budgetService, IMapper mapper)
        {
            _budgetService = budgetService;
            _mapper = mapper;
        }

        /// <summary>
        /// Find upcoming private payments by user ID and budget date 
        /// </summary>
        /// <param name="idUser">ID of user</param>
        /// <param name="date">Budget date </param>
        [HttpGet("upcoming-payments/{idUser}/private")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PrivatePaymentDTO>))]
        public async Task<IActionResult> GetUpcomingPaymentsAsync(int idUser, DateTime date)
        {
            if (idUser <= 0)
            {
                ModelState.AddModelError("error", "User ID is invalid");
                return BadRequest(ModelState);
            }

            var budget = await _budgetService.GetBudgetAsync(idUser, date);
            if (budget == null)
            {
                return NotFound();
            }

            var privatePayments = budget.Payments
                .Where(p => p.PaymentDate != null && p.SharedPayments.All(s => s.IdPayment == 0));
            if (privatePayments == null || privatePayments.Count() == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<PrivatePaymentDTO>>(privatePayments));
        }

        /// <summary>
        /// Find upcoming shared payments with other users by user ID and budget date 
        /// </summary>
        /// <param name="idUser">ID of user</param>
        /// <param name="date">Budget date </param>
        [HttpGet("upcoming-payments/{idUser}/shared")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PrivatePaymentDTO>))]
        public async Task<IActionResult> GetUpcomingSharedPaymentsAsync(int idUser, DateTime date)
        {
            if (idUser <= 0)
            {
                ModelState.AddModelError("error", "User ID is invalid");
                return BadRequest(ModelState);
            }

            var budget = await _budgetService.GetBudgetAsync(idUser, date);
            if (budget == null)
            {
                return NotFound();
            }

            var sharedPayments = budget.Payments
                .Where(p => p.PaymentDate != null && p.SharedPayments.All(s => s.IdPayment > 0));
            if (sharedPayments == null || sharedPayments.Count() == 0)
            {
                return NotFound();
            }

            //...


            return Ok();
        }

        /// <summary>
        /// Find upcoming sharing payments with user by user ID and budget date 
        /// </summary>
        /// <param name="idUser">ID of user</param>
        /// <param name="date">Budget date </param>
        [HttpGet("upcoming-payments/{idUser}/sharing")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PrivatePaymentDTO>))]
        public async Task<IActionResult> GetUpcomingSharingPaymentsAsync(int idUser, DateTime date)
        {
            return Ok();
        }
    }
}
