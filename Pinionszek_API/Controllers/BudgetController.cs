using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pinionszek_API.DbContexts;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDto;
using Pinionszek_API.Models.DTOs.GetDto;
using Pinionszek_API.Services.DatabaseServices.BudgetService;
using System.Collections.Generic;

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
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetPrivatePaymentDto>))]
        public async Task<IActionResult> GetUpcomingPrivatePaymentsAsync(int idUser, DateTime date)
        {
            if (idUser <= 0)
            {
                ModelState.AddModelError("error", "User ID is invalid");
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
                .Where(upd => upd.SharedPayment == null || upd.SharedPayment?.IdSharedPayment == 0);

            if (upcomingPaymentsData == null || upcomingPaymentsData.Count() == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<GetPrivatePaymentDto>>(upcomingPrivatePaymentsData));
        }

        /// <summary>
        /// Find upcoming shared payments with other users by user ID and budget date 
        /// </summary>
        /// <param name="idUser">ID of user</param>
        /// <param name="date">Budget date </param>
        [HttpGet("upcoming-payments/{idUser}/shared")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetSharedPaymentToFriendDto>))]
        public async Task<IActionResult> GetUpcomingPaymentsSharedWithFriendAsync(int idUser, DateTime date)
        {
            if (idUser <= 0)
            {
                ModelState.AddModelError("error", "User ID is invalid");
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
                _mapper.Map(new GetFriendDto
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
        /// Find upcoming payments that are shared for user by user tag and date
        /// </summary>
        /// <param name="userTag">user tag</param>
        /// <param name="date">Payment of year and month</param>
        [HttpGet("upcoming-payments/{userTag}/assigement")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetAssignedPaymentToUserDto>))]
        public async Task<IActionResult> GetUpcomingPaymentsSharedWithUserAsync(int userTag, DateTime date)
        {
            if (userTag <= 0)
            {
                ModelState.AddModelError("error", "User ID is invalid");
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
                .Where(
                    apd => apd.PaymentDate != null &&
                    (apd.PaymentDate >= firstDayOfMonth &&
                    apd.PaymentDate <= lastDayOfMonth)
                ).ToList();
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
                _mapper.Map(new GetFriendDto
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
    }
}
