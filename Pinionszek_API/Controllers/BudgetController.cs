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

        [HttpGet]
        public async Task<IActionResult> GetUpcomingPrivatePaymentsAsync(int id, DateTime date) 
        {
            if (id < 0) 
            {
                return BadRequest();
            }

            Budget? budget = await _budgetService.GetBudgetAsync(id, date);
            if (budget == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<GetPaymentDTO>>(budget.Payments));
        }

    }
}
