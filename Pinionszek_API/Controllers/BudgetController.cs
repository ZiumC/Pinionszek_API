using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pinionszek_API.DbContexts;

namespace Pinionszek_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly ProdDbContext _dbContext;

        public BudgetController(ProdDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetUpcomingPrivatePaymentsAsync(int idBudget, int idUser) 
        {

            return Ok("AA");
        }

    }
}
