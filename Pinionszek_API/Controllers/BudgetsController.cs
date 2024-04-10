using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Pinionszek_API.DbContexts;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDto;
using Pinionszek_API.Models.DTOs.GetDto.Payments;
using Pinionszek_API.Models.DTOs.GetDto.User;
using Pinionszek_API.Services.DatabaseServices.BudgetService;
using Pinionszek_API.Services.DatabaseServices.PaymentService;
using Pinionszek_API.Services.DatabaseServices.UserService;
using Pinionszek_API.Utils;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pinionszek_API.Controllers
{
    [ApiExplorerSettings(GroupName = "Budgets")]
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : ControllerBase
    {
        private readonly IBudgetApiService _budgetService;
        private readonly IPaymentApiService _paymentService;
        private readonly IUserApiService _userService;
        private readonly BudgetUtils _budgetUtils;
        private readonly IMapper _mapper;

        public BudgetsController(IConfiguration _config, IBudgetApiService budgetService,
            IPaymentApiService paymentService, IUserApiService userService, IMapper mapper)
        {
            _budgetUtils = new BudgetUtils(_config);
            _budgetService = budgetService;
            _paymentService = paymentService;
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get budget spendings, status, rules and revenue
        /// </summary>
        /// <param name="idUser">User ID</param>
        /// <param name="date">Budget date</param>
        [HttpGet("summary")]
        [ProducesResponseType(200, Type = typeof(GetBudgetSummaryDto))]
        public async Task<IActionResult> GetBudgetSummaryAsync([Required] DateTime date, [Required] int idUser)
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

            var budgetData = await _budgetService
                .GetBudgetDataAsync(idUser, date);
            if (budgetData == null)
            {
                return NotFound();
            }

            var userSettingsData = await _userService
                .GetUserSettingsAsync(idUser);
            if (userSettingsData == null)
            {
                return NotFound();
            }

            GetBudgetSummaryDto budgetSummaryDto;
            try
            {
                var budgetPaymentsData = await _paymentService.GetPaymentsAsync(budgetData.IdBudget);
                decimal needs = _budgetUtils
                            .GetPaymentsSum(GeneralCatEnum.NEEDS, PaymentColEnum.CHARGE, budgetPaymentsData);

                decimal wants = _budgetUtils
                    .GetPaymentsSum(GeneralCatEnum.WANTS, PaymentColEnum.CHARGE, budgetPaymentsData);

                decimal savings = _budgetUtils
                    .GetPaymentsSum(GeneralCatEnum.SAVINGS, PaymentColEnum.CHARGE, budgetPaymentsData);

                decimal refounds = _budgetUtils
                    .GetPaymentsSum(GeneralCatEnum.NO_CATEGORY, PaymentColEnum.REFOUND, budgetPaymentsData);

                decimal actual = (budgetData.Revenue + budgetData.Surplus + refounds) - (needs + wants + savings);

                var budgetDto = _mapper.Map<GetBudgetDto>(budgetData);
                budgetSummaryDto = _mapper.Map<GetBudgetSummaryDto>(new GetBudgetSummaryDto
                {
                    Budget = budgetDto,
                    Needs = needs,
                    Wants = wants,
                    Savings = savings,
                    Actual = actual
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }

            return Ok(budgetSummaryDto);
        }

        /// <summary>
        /// Get list of budget months by selected year and user ID
        /// </summary>
        /// <param name="idUser">User ID</param>
        /// <param name="year">Budget year</param>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetBudgetSummaryDto>))]
        public async Task<IActionResult> GetBudgetsAsync([Required] int year, [Required] int idUser)
        {
            if (idUser <= 0)
            {
                ModelState.AddModelError("error", "User ID is invalid");
                return BadRequest(ModelState);
            }

            if (year < 1 || year > 9999)
            {
                ModelState.AddModelError("error", "Budget year is invalid");
                return BadRequest(ModelState);
            }

            var budgetsData = await _budgetService.GetBudgetsAsync(idUser);
            if (budgetsData == null || budgetsData.Count() == 0)
            {
                return NotFound();
            }

            var budgetsByYearData = budgetsData
                .Where(bbyd => bbyd.BudgetYear.Year == year)
                .ToList();
            if (budgetsByYearData == null || budgetsByYearData.Count() == 0)
            {
                return NotFound();
            }

            var budgetsSummaryDto = new List<GetBudgetSummaryDto>();
            foreach (var budgetData in budgetsByYearData)
            {
                var budgetPaymentsData = await _paymentService.GetPaymentsAsync(budgetData.IdBudget);
                try
                {
                    decimal needs = _budgetUtils
                        .GetPaymentsSum(GeneralCatEnum.NEEDS, PaymentColEnum.CHARGE, budgetPaymentsData);

                    decimal wants = _budgetUtils
                        .GetPaymentsSum(GeneralCatEnum.WANTS, PaymentColEnum.CHARGE, budgetPaymentsData);

                    decimal savings = _budgetUtils
                        .GetPaymentsSum(GeneralCatEnum.SAVINGS, PaymentColEnum.CHARGE, budgetPaymentsData);

                    decimal refounds = _budgetUtils
                        .GetPaymentsSum(GeneralCatEnum.NO_CATEGORY, PaymentColEnum.REFOUND, budgetPaymentsData);

                    decimal actual = (budgetData.Revenue + budgetData.Surplus + refounds) - (needs + wants + savings);

                    var budgetDto = _mapper.Map<GetBudgetDto>(budgetData);
                    budgetsSummaryDto.Add(_mapper.Map<GetBudgetSummaryDto>(new GetBudgetSummaryDto
                    {
                        Budget = budgetDto,
                        Needs = needs,
                        Wants = wants,
                        Savings = savings,
                        Actual = actual
                    }));

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return StatusCode(500, "Internal server error");
                }
            }

            return Ok(budgetsSummaryDto);
        }
    }
}
