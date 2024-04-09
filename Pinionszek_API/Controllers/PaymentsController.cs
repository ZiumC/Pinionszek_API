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
        [HttpGet("upcoming/private")]
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
                var sharedPaymentData = await _paymentService
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

        /// <summary>
        /// Get upcoming shared payments with other users by userID (that user who share) and budget date 
        /// </summary>
        /// <param name="idUser">User ID</param>
        /// <param name="date">Budget date</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        [HttpGet("upcoming/share")]
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

            var budgetPaymentsData = await _paymentService
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
                var sharedPaymentData = await _paymentService.GetSharedPaymentDataAsync(privatePaymentData.IdPayment);
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
        [HttpGet("upcoming/assigement")]
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

            var assignedPaymentsToUserData = await _paymentService.GetAssignedPaymentsAsync(userTag);
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
                var sharedPaymentData = await _paymentService
                    .GetSharedPaymentDataAsync(idAssignedPayment);
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

            var paymentData = await _paymentService.GetPaymentAsync(idPayment, idUser);
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
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        [HttpGet("private")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetPrivatePaymentDto>))]
        public async Task<IActionResult> GetPrivatePaymentsAsync
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

            var budgetData = await _budgetService.GetBudgetDataAsync(idUser, date);
            if (budgetData == null)
            {
                return NotFound();
            }

            var budgetPaymentsData = await _paymentService.GetPaymentsAsync(budgetData.IdBudget);
            if (budgetPaymentsData == null || budgetPaymentsData.Count() == 0)
            {
                return NotFound();
            }

            foreach (var payment in budgetPaymentsData)
            {
                var sharedPaymentData = await _paymentService
                    .GetSharedPaymentDataAsync(payment.IdPayment);

                payment.SharedPayment = sharedPaymentData;
            }

            var privatePaymentsData = budgetPaymentsData
                .Where(upd => upd.SharedPayment == null ||
                       upd.SharedPayment?.IdSharedPayment == 0)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList(); ;
            if (privatePaymentsData == null || privatePaymentsData.Count() == 0)
            {
                return NotFound();
            }

            var privatePaymentDto = _mapper.Map<IEnumerable<GetPrivatePaymentDto>>(privatePaymentsData);

            return Ok(privatePaymentDto);
        }
    }
}
