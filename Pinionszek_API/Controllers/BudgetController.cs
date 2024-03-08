using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        /// Find payments by user ID and budget date 
        /// </summary>
        /// <param name="idUser">ID of user</param>
        /// <param name="date">Budget date </param>
        [HttpGet("payments/{idUser}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PaymentDTO>))]
        public async Task<IActionResult> GetUpcomingPaymentsAsync(int idUser, DateTime date) 
        {
            if (idUser <= 0) 
            {
                return BadRequest(ModelState);
            }

            var budget = await _budgetService.GetBudgetAsync(idUser, date);
            if (budget == null)
            {
                return NotFound();
            }

            var payments = budget.Payments.Where(p => p.PaymentDate != null);
            if (payments == null || payments.Count() == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<PaymentDTO>>(payments));
        }

    }
}
