using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pinionszek_API.DbContexts;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Services.DatabaseServices.BudgetService;

namespace Pinionszek_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetApiService _budgetService;

        public BudgetController(IBudgetApiService budgetService)
        {
            _budgetService = budgetService;
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

            return Ok(budget);
        }

    }
}
