using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Pinionszek_API.DbContexts;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDto;
using Pinionszek_API.Models.DTOs.GetDto.Payments;
using Pinionszek_API.Models.DTOs.GetDto.User;
using Pinionszek_API.Services.DatabaseServices.BudgetService;
using Pinionszek_API.Utils;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pinionszek_API.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : ControllerBase
    {
        private readonly IBudgetApiService _budgetService;
        private readonly BudgetUtils _budgetUtils;
        private readonly IMapper _mapper;

        public BudgetsController(IConfiguration _config, IBudgetApiService budgetService, IMapper mapper)
        {
            _budgetUtils = new BudgetUtils(_config);
            _budgetService = budgetService;
            _mapper = mapper;
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

            var budgetPaymentsData = await _budgetService
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
                .OrderBy(bpd => bpd.PaymentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            if (upcomingPrivatePaymentsData == null || upcomingPrivatePaymentsData.Count() == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<GetPrivatePaymentDto>>(upcomingPrivatePaymentsData));
        }

        /// <summary>
        /// Get upcoming shared payments with other users by userID (that user who share) and budget date 
        /// </summary>
        /// <param name="idUser">User ID</param>
        /// <param name="date">Budget date</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        [HttpGet("upcoming-payments/share")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetSharedPaymentToFriendDto>))]
        public async Task<IActionResult> GetUpcomingPaymentsSharedWithFriendAsync
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

            var budgetPaymentsData = await _budgetService
                .GetPaymentsAsync(budgetData.IdBudget);
            if (budgetPaymentsData == null)
            {
                return NotFound();
            }

            var upcomingPrivatePaymentsData = budgetPaymentsData
                .Where(p => p.PaymentDate != null)
                .ToList();

            if (upcomingPrivatePaymentsData == null || upcomingPrivatePaymentsData.Count() == 0)
            {
                return NotFound();
            }

            List<GetSharedPaymentToFriendDto> sharedPaymentsDto = new List<GetSharedPaymentToFriendDto>();
            foreach (var privatePaymentData in upcomingPrivatePaymentsData)
            {
                var sharedPaymentData = await _budgetService.GetSharedPaymentDataAsync(privatePaymentData.IdPayment);
                if (sharedPaymentData == null)
                {
                    continue;
                }
                var friendNameAndTag = await _budgetService.GetFriendReceiveNameAndTagAsync(sharedPaymentData.IdSharedPayment);

                var privatePaymentDto = _mapper.Map<GetPrivatePaymentDto>(privatePaymentData);
                var sharedPaymentToFriendDto = _mapper.Map<GetSharedPaymentToFriendDto>(privatePaymentDto);
                _mapper.Map(new GetPaymentFriendDto
                {
                    Name = friendNameAndTag.Item1,
                    FriendTag = friendNameAndTag.Item2,
                }, sharedPaymentToFriendDto);

                sharedPaymentsDto.Add(sharedPaymentToFriendDto);
            }

            //i know this is waste or server resource but this is needed
            //due to properly return pages with proper size
            sharedPaymentsDto = sharedPaymentsDto
                .OrderBy(p => p.Payment.PaymentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            if (sharedPaymentsDto.Count() == 0)
            {
                return NotFound();
            }

            return Ok(sharedPaymentsDto);
        }

        /// <summary>
        /// Get upcoming payments that are shared for user by userTag and payment date
        /// </summary>
        /// <param name="userTag">User tag</param>
        /// <param name="date">Payment year and month</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        [HttpGet("upcoming-payments/assigement")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetAssignedPaymentToUserDto>))]
        public async Task<IActionResult> GetUpcomingPaymentsSharedWithUserAsync
            ([Required] DateTime date, [Required] int userTag, int page = 1, int pageSize = 20)
        {
            if (userTag <= 0)
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

            var assignedPaymentsToUserData = await _budgetService.GetAssignedPaymentsAsync(userTag);
            if (assignedPaymentsToUserData == null || assignedPaymentsToUserData.Count() == 0)
            {
                return NotFound();
            }

            DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddTicks(-1);

            var upcomingAssignedPaymentsData = assignedPaymentsToUserData
                .Where(apd => apd.PaymentDate != null &&
                        (apd.PaymentDate >= firstDayOfMonth &&
                         apd.PaymentDate <= lastDayOfMonth))
                .OrderBy(apd => apd.PaymentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            if (
                    upcomingAssignedPaymentsData == null ||
                    upcomingAssignedPaymentsData.Count() == 0
                )
            {
                return NotFound();
            }

            var assignedPaymentsToUserDto = new List<GetAssignedPaymentToUserDto>();
            foreach (var assignedPaymentData in upcomingAssignedPaymentsData)
            {
                int idAssignedPayment = assignedPaymentData.IdPayment;
                var sharedPaymentData = await _budgetService.GetSharedPaymentDataAsync(idAssignedPayment);
                if (sharedPaymentData == null)
                {
                    continue;
                }

                int idSharedPayment = sharedPaymentData.IdSharedPayment;
                var friendNameAndTag = await _budgetService.GetFriendSenderNameAndTagAsync(idSharedPayment);

                var assignedPaymentDto = _mapper.Map<GetAssignedPaymentDto>(assignedPaymentData);
                var assignedPaymentToUserDto = _mapper.Map<GetAssignedPaymentToUserDto>(assignedPaymentDto);
                _mapper.Map(new GetPaymentFriendDto
                {
                    Name = friendNameAndTag.Item1,
                    FriendTag = friendNameAndTag.Item2,
                }, assignedPaymentToUserDto);

                assignedPaymentsToUserDto.Add(assignedPaymentToUserDto);
            }

            if (assignedPaymentsToUserDto.Count() == 0)
            {
                return NotFound();
            }

            return Ok(assignedPaymentsToUserDto);
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

            var userSettingsData = await _budgetService
                .GetUserSettingsAsync(idUser);
            if (userSettingsData == null)
            {
                return NotFound();
            }

            GetBudgetSummaryDto budgetSummaryDto;
            try
            {
                var budgetPaymentsData = await _budgetService.GetPaymentsAsync(budgetData.IdBudget);
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
                var budgetPaymentsData = await _budgetService.GetPaymentsAsync(budgetData.IdBudget);
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

        /// <summary>
        /// Get payment details by payment ID and user ID
        /// </summary>
        /// <param name="idUser">User ID</param>
        /// <param name="idPayment">Payment ID</param>
        [HttpGet("payments/{idPayment}")]
        [ProducesResponseType(200, Type = typeof(GetPrivatePaymentDto))]
        public async Task<IActionResult> GetPaymentDetailsAsync([Required] int idUser, int idPayment)
        {
            if (idUser <= 0)
            {
                ModelState.AddModelError("error", "User ID is invalid");
                return BadRequest(ModelState);
            }

            if (idPayment <= 0)
            {
                ModelState.AddModelError("error", "Payment ID is invalid");
                return BadRequest(ModelState);
            }

            var paymentData = await _budgetService.GetPaymentAsync(idPayment, idUser);
            if (paymentData == null)
            {
                return NotFound();
            }

            var paymentDto = _mapper.Map<GetPrivatePaymentDto>(paymentData);

            return Ok(paymentDto);
        }

        /// <summary>
        /// Get all private payments by user ID and budget date
        /// </summary>
        /// <param name="idUser">User ID</param>
        /// <param name="date">Budget date</param>
        [HttpGet("payments/private")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetPrivatePaymentDto>))]
        public async Task<IActionResult> GetPrivatePaymentsAsync([Required] DateTime date, [Required] int idUser)
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

            var budgetData = await _budgetService.GetBudgetDataAsync(idUser, date);
            if (budgetData == null)
            {
                return NotFound();
            }

            var budgetPaymentsData = await _budgetService.GetPaymentsAsync(budgetData.IdBudget);
            if (budgetPaymentsData == null || budgetPaymentsData.Count() == 0)
            {
                return NotFound();
            }

            foreach (var payment in budgetPaymentsData)
            {
                var sharedPaymentData = await _budgetService
                    .GetSharedPaymentDataAsync(payment.IdPayment);

                payment.SharedPayment = sharedPaymentData;
            }

            var privatePaymentsData = budgetPaymentsData
                .Where(upd => upd.SharedPayment == null || upd.SharedPayment?.IdSharedPayment == 0);
            if (privatePaymentsData == null || privatePaymentsData.Count() == 0)
            {
                return NotFound();
            }

            var privatePaymentDto = _mapper.Map<IEnumerable<GetPrivatePaymentDto>>(privatePaymentsData);

            return Ok(privatePaymentDto);
        }

        /// <summary>
        /// Get shared payments with other users by userID (that user who share) and budget date 
        /// </summary>
        /// <param name="idUser">User ID</param>
        /// <param name="date">Budget date</param>
        [HttpGet("payments/share")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetSharedPaymentToFriendDto>))]
        public async Task<IActionResult> GetPaymentsSharedWithFriendAsync([Required] DateTime date, [Required] int idUser)
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

            var budgetPaymentsData = await _budgetService
                .GetPaymentsAsync(budgetData.IdBudget);
            if (budgetPaymentsData == null)
            {
                return NotFound();
            }

            List<GetSharedPaymentToFriendDto> sharedPaymentsDto = new List<GetSharedPaymentToFriendDto>();
            foreach (var paymentData in budgetPaymentsData)
            {
                int idPayment = paymentData.IdPayment;
                var sharedPaymentData = await _budgetService.GetSharedPaymentDataAsync(idPayment);
                if (sharedPaymentData == null)
                {
                    continue;
                }

                int idSharedPayment = sharedPaymentData.IdSharedPayment;
                var friendNameAndTag = await _budgetService.GetFriendReceiveNameAndTagAsync(idSharedPayment);

                var privatePaymentDto = _mapper.Map<GetPrivatePaymentDto>(paymentData);
                var sharedPaymentToFriendDto = _mapper.Map<GetSharedPaymentToFriendDto>(privatePaymentDto);
                _mapper.Map(new GetPaymentFriendDto
                {
                    Name = friendNameAndTag.Item1,
                    FriendTag = friendNameAndTag.Item2,
                }, sharedPaymentToFriendDto);

                sharedPaymentsDto.Add(sharedPaymentToFriendDto);
            }

            if (sharedPaymentsDto.Count() == 0)
            {
                return NotFound();
            }

            return Ok(sharedPaymentsDto);
        }

        /// <summary>
        /// Get payments that are shared for user by userTag and payment date
        /// </summary>
        /// <param name="userTag">User tag</param>
        /// <param name="date">Payment year and month</param>
        [HttpGet("payments/assigement")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetAssignedPaymentToUserDto>))]
        public async Task<IActionResult> GetPaymentsSharedWithUserAsync([Required] DateTime date, [Required] int userTag)
        {
            if (userTag <= 0)
            {
                ModelState.AddModelError("error", "User tag is invalid");
                return BadRequest(ModelState);
            }

            if (date == DateTime.MinValue)
            {
                ModelState.AddModelError("error", "Budget date is not specified");
                return BadRequest(ModelState);
            }

            var assignedPaymentsToUserData = await _budgetService.GetAssignedPaymentsAsync(userTag);
            if (assignedPaymentsToUserData == null || assignedPaymentsToUserData.Count() == 0)
            {
                return NotFound();
            }

            var assignedPaymentsToUserDto = new List<GetAssignedPaymentToUserDto>();
            foreach (var assignedPaymentData in assignedPaymentsToUserData)
            {
                int idAssignedPayment = assignedPaymentData.IdPayment;
                var sharedPaymentData = await _budgetService.GetSharedPaymentDataAsync(idAssignedPayment);
                if (sharedPaymentData == null)
                {
                    continue;
                }

                int idSharedPayment = sharedPaymentData.IdSharedPayment;
                var friendNameAndTag = await _budgetService.GetFriendSenderNameAndTagAsync(idSharedPayment);

                var assignedPaymentDto = _mapper.Map<GetAssignedPaymentDto>(assignedPaymentData);
                var assignedPaymentToUserDto = _mapper.Map<GetAssignedPaymentToUserDto>(assignedPaymentDto);
                _mapper.Map(new GetPaymentFriendDto
                {
                    Name = friendNameAndTag.Item1,
                    FriendTag = friendNameAndTag.Item2,
                }, assignedPaymentToUserDto);

                assignedPaymentsToUserDto.Add(assignedPaymentToUserDto);
            }

            if (assignedPaymentsToUserDto.Count() == 0)
            {
                return NotFound();
            }

            return Ok(assignedPaymentsToUserDto);
        }

        /// <summary>
        /// Get default general payment categories
        /// </summary>
        [HttpGet("payment-categories/default")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetGeneralCategoryDto>))]
        public async Task<IActionResult> GetDefaultGeneralCategoriesAsync()
        {
            var defaultCategories = await _budgetService.GetDefaultGeneralCategoriesAsync();
            if (defaultCategories == null || defaultCategories.Count() == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<GetGeneralCategoryDto>>(defaultCategories));
        }
    }
}
