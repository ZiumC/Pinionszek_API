using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pinionszek_API.DbContexts;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDTO;
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

            var upcomingPaymentsData = budgetPaymentsData
                .Where(bpd => bpd.PaymentDate != null);

            foreach (var payment in upcomingPaymentsData) 
            {
                var sharedPaymentData = await _budgetService
                    .GetSharedPaymentDataAsync(payment.IdPayment);

                payment.SharedPayment = sharedPaymentData;
            }

            var upcomingPrivatePaymentsData = upcomingPaymentsData
                .Where(upd => upd.SharedPayment == null || upd.SharedPayment?.IdSharedPayment == 0);


            return Ok(_mapper.Map<IEnumerable<GetPrivatePaymentDto>>(upcomingPrivatePaymentsData));
        }

        /// <summary>
        /// Find upcoming shared payments with other users by user ID and budget date 
        /// </summary>
        /// <param name="idUser">ID of user</param>
        /// <param name="date">Budget date </param>
        [HttpGet("upcoming-payments/{idUser}/shared")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetSharedPaymentDto>))]
        public async Task<IActionResult> GetUpcomingSharedPaymentsAsync(int idUser, DateTime date)
        {
            if (idUser <= 0)
            {
                ModelState.AddModelError("error", "User ID is invalid");
                return BadRequest(ModelState);
            }

            var budget = await _budgetService.GetBudgetDataAsync(idUser, date);
            if (budget == null)
            {
                return NotFound();
            }

            var privatePayments = budget.Payments
                .Where(p => p.PaymentDate != null && p.SharedPayment.IdPayment > 0).ToList();
            if (privatePayments == null || privatePayments.Count() == 0)
            {
                return NotFound();
            }

            List<GetSharedPaymentDto> sharedPaymentsDto = new List<GetSharedPaymentDto>();
            foreach (var privatePayment in privatePayments)
            {
                int idSharedPayment = privatePayment.SharedPayment.IdSharedPayment;
                var friendNameAndTag = await _budgetService.GetFriendNameAndTagAsync(idSharedPayment);

                var privatePaymentDto = _mapper.Map<GetPrivatePaymentDto>(privatePayment);
                var sharedPaymentDto = _mapper.Map<GetSharedPaymentDto>(privatePaymentDto);
                _mapper.Map(new GetFriendDto
                {
                    SharredTo = friendNameAndTag.Item1,
                    FriendTag = friendNameAndTag.Item2,
                }, sharedPaymentDto);

                sharedPaymentsDto.Add(sharedPaymentDto);

            }

            return Ok(sharedPaymentsDto);
        }

        /// <summary>
        /// Find upcoming sharing payments with user by user ID and budget date 
        /// </summary>
        /// <param name="idUser">ID of user</param>
        /// <param name="date">Budget date </param>
        [HttpGet("upcoming-payments/{idUser}/sharing")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetPrivatePaymentDto>))]
        public async Task<IActionResult> GetUpcomingSharingPaymentsAsync(int idUser, DateTime date)
        {
            return Ok();
        }
    }
}
