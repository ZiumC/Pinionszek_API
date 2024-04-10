using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Pinionszek_API.Controllers;
using Pinionszek_API.Models.DTOs.GetDto.Payments;
using Pinionszek_API.Profiles;
using Pinionszek_API.Services.DatabaseServices.BudgetService;
using Pinionszek_API.Services.DatabaseServices.PaymentService;
using Pinionszek_API.Tests.DbContexts;

namespace Pinionszek_API.Tests.Tests.IntegrationTests
{
    public class PaymentsControllerTests
    {
        private readonly int _user_1 = 1;
        private readonly int _user_2 = 2;
        private readonly int _user_3 = 3;
        private readonly int _user_4 = 4;
        private readonly int _user_5 = 100;
        private readonly int _defaultPageSize = 1;
        private readonly int _friend_1 = 1001;
        private readonly int _friend_2 = 1002;
        private readonly int _friend_3 = 1003;
        private readonly int _friend_4 = 1004;
        private readonly DateTime _budgetDate;
        private readonly IMapper _mapper;

        public PaymentsControllerTests()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PaymentProfile());
                cfg.AddProfile(new CategoryProfile());
                cfg.AddProfile(new BudgetProfile());
            });
            _mapper = mockMapper.CreateMapper();
            _budgetDate = DateTime.Parse("2024-01-01");
        }

        [Fact]
        public async Task PaymentsController_GetUpcomingPrivatePaymentsAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var paymentApiService = new PaymentApiService(await dbContext);
            var paymentsController = new PaymentsController(paymentApiService, budgetApiService, _mapper);
            var pages = new { page0 = 0, page1 = 1, page2 = 2 };

            //Act
            var okRequest_1 = await paymentsController.GetUpcomingPrivatePaymentsAsync
                (_budgetDate, _user_1, pages.page1, _defaultPageSize);
            var paymentsResult_1 = (okRequest_1 as OkObjectResult)?.Value as IEnumerable<GetPrivatePaymentDto>;

            var okRequest_2 = await paymentsController.GetUpcomingPrivatePaymentsAsync(_budgetDate, _user_2);
            var paymentsResult_2 = (okRequest_2 as OkObjectResult)?.Value as IEnumerable<GetPrivatePaymentDto>;

            var okRequestPage_1 = await paymentsController.GetUpcomingPrivatePaymentsAsync
                (_budgetDate, _user_2, pages.page1, _defaultPageSize);
            var paymentsResultPage_1 = (okRequestPage_1 as OkObjectResult)?.Value as IEnumerable<GetPrivatePaymentDto>;

            var okRequestPage_2 = await paymentsController.GetUpcomingPrivatePaymentsAsync
                (_budgetDate, _user_2, pages.page2, _defaultPageSize);
            var paymentsResultPage_2 = (okRequestPage_2 as OkObjectResult)?.Value as IEnumerable<GetPrivatePaymentDto>;

            var notFoundRequest_1 = await paymentsController.GetUpcomingPrivatePaymentsAsync(_budgetDate, _user_3);
            var notFoundRequest_2 = await paymentsController.GetUpcomingPrivatePaymentsAsync(_budgetDate, _user_4);

            var badRequest_1 = await paymentsController.GetUpcomingPrivatePaymentsAsync(_budgetDate, -_user_5);
            var badRequestResult_1 = (badRequest_1 as BadRequestObjectResult)?.Value as string;

            var badRequest_2 = await paymentsController.GetUpcomingPrivatePaymentsAsync(new DateTime(), _user_5);
            var badRequestResult_2 = (badRequest_2 as BadRequestObjectResult)?.Value as string;

            var badRequest_3 = await paymentsController.GetUpcomingPrivatePaymentsAsync
                (_budgetDate, _user_1, pages.page0, _defaultPageSize);
            var badRequestResult_3 = (badRequest_3 as BadRequestObjectResult)?.Value as string;

            var badRequest_4 = await paymentsController.GetUpcomingPrivatePaymentsAsync
                (_budgetDate, _user_1, pages.page1, -_defaultPageSize);
            var badRequestResult_4 = (badRequest_4 as BadRequestObjectResult)?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            paymentsResult_1.Should().NotBeNullOrEmpty();
            paymentsResult_1?.Count().Should().Be(1);
            paymentsResult_1?
                .Where(pr => string.IsNullOrEmpty(pr.Status))
                .ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(pr => string.IsNullOrEmpty(pr.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Category.GeneralName))
                .ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(pr => pr.IdSharedPayment != null ||
                          pr.IdSharedPayment > 0)
                .ToList().Should().BeNullOrEmpty();


            okRequest_2.Should().BeOfType<OkObjectResult>();
            paymentsResult_2.Should().NotBeNullOrEmpty();
            paymentsResult_2?.Count().Should().Be(2);
            paymentsResult_2?
                .Where(pr => string.IsNullOrEmpty(pr.Status)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2?
                .Where(pr => string.IsNullOrEmpty(pr.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2?
                .Where(pr => pr.IdSharedPayment != null ||
                          pr.IdSharedPayment > 0
                ).ToList().Should().BeNullOrEmpty();

            okRequestPage_1.Should().BeOfType<OkObjectResult>();
            paymentsResultPage_1.Should().NotBeNullOrEmpty();
            paymentsResultPage_1?.Count().Should().Be(1);
            paymentsResultPage_1?
                .Where(pr => string.IsNullOrEmpty(pr.Status))
                .ToList().Should().BeNullOrEmpty();
            paymentsResultPage_1?
                .Where(pr => string.IsNullOrEmpty(pr.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResultPage_1?
                .Where(pr => pr.IdSharedPayment != null ||
                          pr.IdSharedPayment > 0
                ).ToList().Should().BeNullOrEmpty();
            paymentsResultPage_1?
                .Where(pr => pr.IdPayment == 13)
                .ToList().Count().Should().Be(1);

            okRequestPage_2.Should().BeOfType<OkObjectResult>();
            paymentsResultPage_2.Should().NotBeNullOrEmpty();
            paymentsResultPage_2?.Count().Should().Be(1);
            paymentsResultPage_2?
                .Where(pr => string.IsNullOrEmpty(pr.Status)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResultPage_2?
                .Where(pr => string.IsNullOrEmpty(pr.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResultPage_2?
                .Where(pr => pr.IdSharedPayment != null ||
                          pr.IdSharedPayment > 0
                ).ToList().Should().BeNullOrEmpty();
            paymentsResultPage_2?
                .Where(pr => pr.IdPayment == 7)
                .ToList().Count().Should().Be(1);

            notFoundRequest_1.Should().BeOfType<NotFoundResult>();
            notFoundRequest_2.Should().BeOfType<NotFoundResult>();


            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_2?.Contains("is not specified").Should().BeTrue();

            badRequest_3.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_3?.Contains("is is invalid").Should().BeTrue();

            badRequest_4.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_4?.Contains("is is invalid").Should().BeTrue();
        }

        [Fact]
        public async Task PaymentsController_GetUpcomingPaymentsSharedWithFriendAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var paymentApiService = new PaymentApiService(await dbContext);
            var paymentsController = new PaymentsController(paymentApiService, budgetApiService, _mapper);
            var pages = new { page0 = 0, page1 = 1, page2 = 2 };

            //Act
            var okRequest_1 = await paymentsController.GetUpcomingPaymentsSharedWithFriendAsync(_budgetDate, _user_1);
            var paymentsResult_1 = (okRequest_1 as OkObjectResult)?.Value as IEnumerable<GetSharedPaymentToFriendDto>;

            var okRequestPage_1 = await paymentsController.GetUpcomingPaymentsSharedWithFriendAsync
                (_budgetDate, _user_1, pages.page1, _defaultPageSize);
            var paymentsPage_1 = (okRequestPage_1 as OkObjectResult)?.Value as IEnumerable<GetSharedPaymentToFriendDto>;

            var okRequestPage_2 = await paymentsController.GetUpcomingPaymentsSharedWithFriendAsync
                (_budgetDate, _user_1, pages.page2, _defaultPageSize);
            var paymentsPage_2 = (okRequestPage_2 as OkObjectResult)?.Value as IEnumerable<GetSharedPaymentToFriendDto>;

            var notFoundRequest_1 = await paymentsController.GetUpcomingPaymentsSharedWithFriendAsync(_budgetDate, _user_2);
            var notFoundRequest_2 = await paymentsController.GetUpcomingPaymentsSharedWithFriendAsync(_budgetDate, _user_3);
            var notFoundRequest_3 = await paymentsController.GetUpcomingPaymentsSharedWithFriendAsync(_budgetDate, _user_4);

            var badRequest_1 = await paymentsController.GetUpcomingPaymentsSharedWithFriendAsync(_budgetDate, -_user_5);
            var badRequestResult_1 = (badRequest_1 as BadRequestObjectResult)?.Value as string;

            var badRequest_2 = await paymentsController.GetUpcomingPaymentsSharedWithFriendAsync(new DateTime(), _user_5);
            var badRequestResult_2 = (badRequest_2 as BadRequestObjectResult)?.Value as string;

            var badRequest_3 = await paymentsController.GetUpcomingPaymentsSharedWithFriendAsync
                (_budgetDate, _user_1, pages.page0, _defaultPageSize);
            var badRequestResult_3 = (badRequest_3 as BadRequestObjectResult)?.Value as string;

            var badRequest_4 = await paymentsController.GetUpcomingPaymentsSharedWithFriendAsync
                (_budgetDate, _user_1, pages.page1, -_defaultPageSize);
            var badRequestResult_4 = (badRequest_4 as BadRequestObjectResult)?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            paymentsResult_1.Should().NotBeNullOrEmpty();
            paymentsResult_1?.Count().Should().Be(2);
            paymentsResult_1?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(pr => pr.Payment.IdSharedPayment != null ||
                          pr.Payment.IdSharedPayment > 0
                ).ToList().Should().NotBeNullOrEmpty();
            paymentsResult_1?
                .Where(pr => pr.TargetFriend == null
                ).ToList().Should().BeNullOrEmpty();

            okRequestPage_1.Should().BeOfType<OkObjectResult>();
            paymentsPage_1.Should().NotBeNullOrEmpty();
            paymentsPage_1?.Count().Should().Be(1);
            paymentsPage_1?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList().Should().BeNullOrEmpty();
            paymentsPage_1?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsPage_1?
                .Where(pr => pr.Payment.IdSharedPayment != null ||
                          pr.Payment.IdSharedPayment > 0
                ).ToList().Should().NotBeNullOrEmpty();
            paymentsPage_1?
                .Where(pr => pr.TargetFriend == null
                ).ToList() .Should().BeNullOrEmpty();
            paymentsPage_1?
                .Where(pr => pr.Payment.IdPayment == 4)
                .ToList().Count()
                .Should().Be(1);

            okRequestPage_2.Should().BeOfType<OkObjectResult>();
            paymentsPage_2.Should().NotBeNullOrEmpty();
            paymentsPage_2?.Count().Should().Be(1);
            paymentsPage_2?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Status))
                .ToList().Should().BeNullOrEmpty();
            paymentsPage_2?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsPage_2?
                .Where(pr => pr.Payment.IdSharedPayment != null ||
                          pr.Payment.IdSharedPayment > 0
                ).ToList().Should().NotBeNullOrEmpty();
            paymentsPage_2?
                .Where(pr => pr.TargetFriend == null)
                .ToList().Should().BeNullOrEmpty();
            paymentsPage_2?
                .Where(pr => pr.Payment.IdPayment == 1)
                .ToList().Count().Should().Be(1);


            notFoundRequest_1.Should().BeOfType<NotFoundResult>();
            notFoundRequest_2.Should().BeOfType<NotFoundResult>();
            notFoundRequest_3.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_2?.Contains("is not specified").Should().BeTrue();

            badRequest_3.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_3?.Contains("is invalid").Should().BeTrue();

            badRequest_4.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_4?.Contains("is invalid").Should().BeTrue();
        }

        [Fact]
        public async Task PaymentsController_GetUpcomingPaymentsSharedWithUserAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var paymentApiService = new PaymentApiService(await dbContext);
            var paymentsController = new PaymentsController(paymentApiService, budgetApiService, _mapper);
            var pages = new { page0 = 0, page1 = 1, page2 = 2 };

            //Act
            var okRequest_1 = await paymentsController.GetUpcomingPaymentsSharedWithUserAsync(_budgetDate, _friend_2);
            var paymentsResult_1 = (okRequest_1 as OkObjectResult)?.Value as IEnumerable<GetAssignedPaymentToUserDto>;

            var okRequestPage_1 = await paymentsController.GetUpcomingPaymentsSharedWithUserAsync
                (_budgetDate, _friend_2, pages.page1, _defaultPageSize);
            var paymentsResult_2_page_1 = (okRequestPage_1 as OkObjectResult)?.Value as IEnumerable<GetAssignedPaymentToUserDto>;

            var okRequestPage_2 = await paymentsController.GetUpcomingPaymentsSharedWithUserAsync
                (_budgetDate, _friend_2, pages.page2, _defaultPageSize);
            var paymentsResult_2_page_2 = (okRequestPage_2 as OkObjectResult)?.Value as IEnumerable<GetAssignedPaymentToUserDto>;

            var notFoundRequest_1 = await paymentsController.GetUpcomingPaymentsSharedWithUserAsync(_budgetDate, _friend_1);
            var notFoundRequest_2 = await paymentsController.GetUpcomingPaymentsSharedWithUserAsync(_budgetDate, _friend_3);
            var notFoundRequest_3 = await paymentsController.GetUpcomingPaymentsSharedWithUserAsync(_budgetDate, _friend_4);

            var badRequest_1 = await paymentsController.GetUpcomingPaymentsSharedWithUserAsync(_budgetDate, -_friend_1);
            var badRequestResult_1 = (badRequest_1 as BadRequestObjectResult)?.Value as string;

            var badRequest_2 = await paymentsController.GetUpcomingPaymentsSharedWithUserAsync(new DateTime(), _friend_1);
            var badRequestResult_2 = (badRequest_2 as BadRequestObjectResult)?.Value as string;

            var badRequest_3 = await paymentsController.GetUpcomingPaymentsSharedWithUserAsync
                (_budgetDate, _friend_2, pages.page0, _defaultPageSize);
            var badRequestResult_3 = (badRequest_3 as BadRequestObjectResult)?.Value as string;

            var badRequest_4 = await paymentsController.GetUpcomingPaymentsSharedWithUserAsync
                (_budgetDate, _friend_2, pages.page1, -_defaultPageSize);
            var badRequestResult_4 = (badRequest_4 as BadRequestObjectResult)?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            paymentsResult_1.Should().NotBeNullOrEmpty();
            paymentsResult_1?.Count().Should().Be(2);
            paymentsResult_1?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(pr => pr.Payment.IdSharedPayment <= 0
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(pr => pr.Payment.PaymentDate != null &&
                    (pr.Payment.PaymentDate <= DateTime.Parse("2024-01-01")
                    && pr.Payment.PaymentDate >= DateTime.Parse("2024-01-31"))
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(pr => pr.SourceFriend == null
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(pr => pr.SourceFriend.FriendTag == 1002)
                .ToList().Count().Should().Be(0);

            okRequestPage_1.Should().BeOfType<OkObjectResult>();
            paymentsResult_2_page_1.Should().NotBeNullOrEmpty();
            paymentsResult_2_page_1?.Count().Should().Be(1);
            paymentsResult_2_page_1?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2_page_1?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2_page_1?
                .Where(pr => pr.Payment.IdSharedPayment <= 0
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2_page_1?
                .Where(pr => pr.Payment.PaymentDate != null &&
                    (pr.Payment.PaymentDate <= DateTime.Parse("2024-01-01")
                    && pr.Payment.PaymentDate >= DateTime.Parse("2024-01-31"))
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2_page_1?
                .Where(pr => pr.SourceFriend == null
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2_page_1?
                .Where(pr => pr.SourceFriend.FriendTag == 1002)
                .ToList().Count().Should().Be(0);
            paymentsResult_2_page_1?
                .Where(pr => pr.Payment.IdPayment == 4)
                .ToList().Count().Should().Be(1);

            okRequestPage_2.Should().BeOfType<OkObjectResult>();
            paymentsResult_2_page_2.Should().NotBeNullOrEmpty();
            paymentsResult_2_page_2?.Count().Should().Be(1);
            paymentsResult_2_page_2?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2_page_2?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2_page_2?
                .Where(pr => pr.Payment.IdSharedPayment <= 0
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2_page_2?
                .Where(pr => pr.Payment.PaymentDate != null &&
                    (pr.Payment.PaymentDate <= DateTime.Parse("2024-01-01")
                    && pr.Payment.PaymentDate >= DateTime.Parse("2024-01-31"))
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2_page_2?
                .Where(pr => pr.SourceFriend == null
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2_page_2?
                .Where(pr => pr.SourceFriend.FriendTag == 1002)
                .ToList().Count().Should().Be(0);
            paymentsResult_2_page_2?
                .Where(pr => pr.Payment.IdPayment == 1)
                .ToList().Count().Should().Be(1);

            notFoundRequest_1.Should().BeOfType<NotFoundResult>();
            notFoundRequest_2.Should().BeOfType<NotFoundResult>();
            notFoundRequest_3.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_2?.Contains("is not specified").Should().BeTrue();

            badRequest_3.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_3?.Contains("is invalid").Should().BeTrue();

            badRequest_4.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_4?.Contains("is invalid").Should().BeTrue();
        }

        [Fact]
        public async Task PaymentsController_GetPaymentDetailsAsync_ReturnsPaymentOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var paymentApiService = new PaymentApiService(await dbContext);
            var paymentsController = new PaymentsController(paymentApiService, budgetApiService, _mapper);
            int firstPaymentId = 1;
            int lastPaymentId = 13;
            var user1requests = new List<OkObjectResult?>();
            var user2requests = new List<OkObjectResult?>();
            var paymentDetails = new List<GetPrivatePaymentDto?>();

            //Act
            for (int id = firstPaymentId; id <= lastPaymentId; id++)
            {
                if (id <= 6 || id == 12)
                {
                    var okRequest = await paymentsController.GetPaymentDetailsAsync(1, id);
                    var okActionResult = okRequest as OkObjectResult;
                    var paymentDetail = okActionResult?.Value as GetPrivatePaymentDto;
                    user1requests.Add(okActionResult);
                    paymentDetails.Add(paymentDetail);
                }

                if (id > 6 && id <= 11 || id == 13)
                {
                    var okRequest = await paymentsController.GetPaymentDetailsAsync(2, id);
                    var okActionResult = okRequest as OkObjectResult;
                    var paymentDetail = okActionResult?.Value as GetPrivatePaymentDto;
                    user2requests.Add(okActionResult);
                    paymentDetails.Add(paymentDetail);
                }

            }

            //Assert
            user1requests.Should().NotBeNullOrEmpty();
            user1requests.Should().HaveCount(7);
            foreach (var request in user1requests)
            {
                request.Should().BeOfType<OkObjectResult>();
            }

            user2requests.Should().NotBeNullOrEmpty();
            user2requests.Should().HaveCount(6);
            foreach (var request in user2requests)
            {
                request.Should().BeOfType<OkObjectResult>();
            }

            paymentDetails.Should().NotBeNullOrEmpty();
            paymentDetails.Should().HaveCount(13);
            foreach (var payment in paymentDetails)
            {
                payment.Should().BeOfType<GetPrivatePaymentDto>();
                payment.Should().NotBeNull();
                payment?.IdPayment.Should().BeGreaterThan(0);
                payment?.Name.Should().NotBeNullOrEmpty();
                payment?.Charge.Should().BeGreaterThanOrEqualTo(0);
                payment?.Refund.Should().BeGreaterThanOrEqualTo(0);
                payment?.Status.Should().NotBeNullOrEmpty();
                payment?.Category.Should().NotBeNull();
                payment?.Category.GeneralName.Should().NotBeNullOrEmpty();
                payment?.Category.DetailedName.Should().NotBeNullOrEmpty();

                var paymentId = payment?.IdPayment;
                if (paymentId == 1 || paymentId == 4 || paymentId == 10)
                {
                    payment?.IdSharedPayment.Should().BeGreaterThan(0);
                }
            }
        }

        [Fact]
        public async Task PaymentsController_GetPrivatePaymentsAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var paymentApiService = new PaymentApiService(await dbContext);
            var budgetController = new PaymentsController(paymentApiService, budgetApiService, _mapper);
            var pages = new { page0 = 0, page1 = 1, page2 = 2 };
            int pageSize = 3;

            //Act
            var okRequest_1 = await budgetController.GetPrivatePaymentsAsync(_budgetDate, _user_1);
            var paymentsResult_1 = (okRequest_1 as OkObjectResult)?.Value as List<GetPrivatePaymentDto>;

            var okRequest_2 = await budgetController.GetPrivatePaymentsAsync(_budgetDate, _user_2);
            var paymentsResult_2 = (okRequest_2 as OkObjectResult)?.Value as List<GetPrivatePaymentDto>;

            var okRequestPage_1 = await budgetController.GetPrivatePaymentsAsync
                (_budgetDate, _user_1, pages.page1, pageSize);
            var paymentsResultPage_1 = (okRequestPage_1 as OkObjectResult)?.Value as List<GetPrivatePaymentDto>;

            var okRequestPage_2 = await budgetController.GetPrivatePaymentsAsync
                (_budgetDate, _user_1, pages.page2, pageSize);
            var paymentsResultPage_2 = (okRequestPage_2 as OkObjectResult)?.Value as List<GetPrivatePaymentDto>;

            var notfoundRequest_1 = await budgetController.GetPrivatePaymentsAsync(_budgetDate, _user_3);

            var badRequest_1 = await budgetController.GetPrivatePaymentsAsync(_budgetDate, -_user_4);
            var badRequestResult_1 = (badRequest_1 as BadRequestObjectResult)?.Value as string;

            var badRequest_2 = await budgetController.GetPrivatePaymentsAsync(new DateTime(), _user_1);
            var badRequestResult_2 = (badRequest_2 as BadRequestObjectResult)?.Value as string;

            var badRequest_3 = await budgetController.GetPrivatePaymentsAsync
                (_budgetDate, _user_1, pages.page0, _defaultPageSize);
            var badRequestResult_3 = (badRequest_3 as BadRequestObjectResult)?.Value as string;

            var badRequest_4 = await budgetController.GetPrivatePaymentsAsync
                (_budgetDate, _user_1, pages.page1, -_defaultPageSize);
            var badRequestResult_4 = (badRequest_4 as BadRequestObjectResult)?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            paymentsResult_1.Should().NotBeNull();
            paymentsResult_1.Should().BeOfType<List<GetPrivatePaymentDto>>();
            paymentsResult_1?.Count().Should().Be(5);
            paymentsResult_1?
                .Where(pr => pr.IdPayment <= 0)
                .Should().BeEmpty();
            paymentsResult_1?
                .Where(pr => string.IsNullOrEmpty(pr.Name))
                .Should().BeEmpty();
            paymentsResult_1?
                .Where(pr => pr.Charge >= 0 || pr.Refund >= 0)
                .Should().NotBeEmpty();
            paymentsResult_1?
                .Where(pr => string.IsNullOrEmpty(pr.Status))
                .Should().BeEmpty();
            paymentsResult_1?
                .Where(pr => pr.Category == null ||
                    string.IsNullOrEmpty(pr.Category.DetailedName) ||
                    string.IsNullOrEmpty(pr.Category.GeneralName))
                .Should().BeEmpty();

            okRequest_2.Should().BeOfType<OkObjectResult>();
            paymentsResult_2.Should().NotBeNull();
            paymentsResult_2.Should().BeOfType<List<GetPrivatePaymentDto>>();
            paymentsResult_2?.Count().Should().Be(5);
            paymentsResult_2?
                .Where(pr => pr.IdPayment <= 0)
                .Should().BeEmpty();
            paymentsResult_2?
                .Where(pr => string.IsNullOrEmpty(pr.Name))
                .Should().BeEmpty();
            paymentsResult_2?
                .Where(pr => pr.Charge >= 0 || pr.Refund >= 0)
                .Should().NotBeEmpty();
            paymentsResult_2?
                .Where(pr => string.IsNullOrEmpty(pr.Status))
                .Should().BeEmpty();
            paymentsResult_2?
                .Where(pr => pr.Category == null ||
                    string.IsNullOrEmpty(pr.Category.DetailedName) ||
                    string.IsNullOrEmpty(pr.Category.GeneralName))
                .Should().BeEmpty();

            okRequestPage_1.Should().BeOfType<OkObjectResult>();
            paymentsResultPage_1.Should().NotBeNull();
            paymentsResultPage_1.Should().BeOfType<List<GetPrivatePaymentDto>>();
            paymentsResultPage_1?.Count().Should().Be(3);
            paymentsResultPage_1?
                .Where(pr => pr.IdPayment <= 0)
                .Should().BeEmpty();
            paymentsResultPage_1?
                .Where(pr => string.IsNullOrEmpty(pr.Name))
                .Should().BeEmpty();
            paymentsResultPage_1?
                .Where(pr => pr.Charge >= 0 || pr.Refund >= 0)
                .Should().NotBeEmpty();
            paymentsResultPage_1?
                .Where(pr => string.IsNullOrEmpty(pr.Status))
                .Should().BeEmpty();
            paymentsResultPage_1?
                .Where(pr => pr.Category == null ||
                    string.IsNullOrEmpty(pr.Category.DetailedName) ||
                    string.IsNullOrEmpty(pr.Category.GeneralName))
                .Should().BeEmpty();

            okRequestPage_2.Should().BeOfType<OkObjectResult>();
            paymentsResultPage_2.Should().NotBeNull();
            paymentsResultPage_2.Should().BeOfType<List<GetPrivatePaymentDto>>();
            paymentsResultPage_2?.Count().Should().Be(2);
            paymentsResultPage_2?
                .Where(pr => pr.IdPayment <= 0)
                .Should().BeEmpty();
            paymentsResultPage_2?
                .Where(pr => string.IsNullOrEmpty(pr.Name))
                .Should().BeEmpty();
            paymentsResultPage_2?
                .Where(pr => pr.Charge >= 0 || pr.Refund >= 0)
                .Should().NotBeEmpty();
            paymentsResultPage_2?
                .Where(pr => string.IsNullOrEmpty(pr.Status))
                .Should().BeEmpty();
            paymentsResultPage_2?
                .Where(pr => pr.Category == null ||
                string.IsNullOrEmpty(pr.Category.DetailedName) ||
                string.IsNullOrEmpty(pr.Category.GeneralName))
                .Should().BeEmpty();

            notfoundRequest_1.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_2?.Contains("is not specified").Should().BeTrue();

            badRequest_3.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_3?.Contains("is invalid").Should().BeTrue();

            badRequest_4.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_4?.Contains("is invalid").Should().BeTrue();
        }

        [Fact]
        public async Task PaymentsController_GetPaymentsSharedWithFriendAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var paymentApiService = new PaymentApiService(await dbContext);
            var paymentsController = new PaymentsController(paymentApiService, budgetApiService, _mapper);
            var pages = new { page0 = 0, page1 = 1, page2 = 2 };

            //Act
            var okRequest_1 = await paymentsController.GetPaymentsSharedWithFriendAsync(_budgetDate, _user_1);
            var paymentsResult_1 = (okRequest_1 as OkObjectResult)?.Value as IEnumerable<GetSharedPaymentToFriendDto>;

            var okRequest_2 = await paymentsController.GetPaymentsSharedWithFriendAsync(_budgetDate, _user_2);
            var paymentsResult_2 = (okRequest_2 as OkObjectResult)?.Value as IEnumerable<GetSharedPaymentToFriendDto>;

            var okRequestPage_1 = await paymentsController.GetPaymentsSharedWithFriendAsync
                (_budgetDate, _user_1, pages.page1, _defaultPageSize);
            var paymentsResultPage_1 = (okRequestPage_1 as OkObjectResult)?.Value as IEnumerable<GetSharedPaymentToFriendDto>;

            var okRequestPage_2 = await paymentsController.GetPaymentsSharedWithFriendAsync
                (_budgetDate, _user_1, pages.page2, _defaultPageSize);
            var paymentsResultPage_2 = (okRequestPage_2 as OkObjectResult)?.Value as IEnumerable<GetSharedPaymentToFriendDto>;

            var notFoundRequest_2 = await paymentsController.GetPaymentsSharedWithFriendAsync(_budgetDate, _user_3);
            var notFoundRequest_3 = await paymentsController.GetPaymentsSharedWithFriendAsync(_budgetDate, _user_4);

            var badRequest_1 = await paymentsController.GetPaymentsSharedWithFriendAsync(_budgetDate, -_user_5);
            var badRequestResult_1 = (badRequest_1 as BadRequestObjectResult)?.Value as string;

            var badRequest_2 = await paymentsController.GetPaymentsSharedWithFriendAsync(new DateTime(), _user_5);
            var badRequestResult_2 = (badRequest_2 as BadRequestObjectResult)?.Value as string;

            var badRequest_3 = await paymentsController.GetPaymentsSharedWithFriendAsync
                (_budgetDate, _user_1, pages.page0, _defaultPageSize);
            var badRequestResult_3 = (badRequest_3 as BadRequestObjectResult)?.Value as string;

            var badRequest_4 = await paymentsController.GetPaymentsSharedWithFriendAsync
                (_budgetDate, _user_1, pages.page1, -_defaultPageSize);
            var badRequestResult_4 = (badRequest_4 as BadRequestObjectResult)?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            paymentsResult_1.Should().NotBeNullOrEmpty();
            paymentsResult_1?.Count().Should().Be(2);
            paymentsResult_1?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Status))
                .ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName))
                .ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(pr => pr.Payment.IdSharedPayment != null ||
                          pr.Payment.IdSharedPayment > 0)
                .ToList().Should().NotBeNullOrEmpty();
            paymentsResult_1?
                .Where(pr => pr.TargetFriend == null)
                .ToList().Should().BeNullOrEmpty();

            okRequest_2.Should().BeOfType<OkObjectResult>();
            paymentsResult_2.Should().NotBeNullOrEmpty();
            paymentsResult_2?.Count().Should().Be(1);
            paymentsResult_2?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Status))
                .ToList().Should().BeNullOrEmpty();
            paymentsResult_2?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2?
                .Where(pr => pr.Payment.IdSharedPayment != null ||
                          pr.Payment.IdSharedPayment > 0
                ).ToList().Should().NotBeNullOrEmpty();
            paymentsResult_2?
                .Where(pr => pr.TargetFriend == null
                ).ToList().Should().BeNullOrEmpty();

            okRequestPage_1.Should().BeOfType<OkObjectResult>();
            paymentsResultPage_1.Should().NotBeNullOrEmpty();
            paymentsResultPage_1?.Count().Should().Be(1);
            paymentsResultPage_1?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Status))
                .ToList().Should().BeNullOrEmpty();
            paymentsResultPage_1?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName))
                .ToList().Should().BeNullOrEmpty();
            paymentsResultPage_1?
                .Where(pr => pr.Payment.IdSharedPayment != null ||
                          pr.Payment.IdSharedPayment > 0)
                .ToList().Should().NotBeNullOrEmpty();
            paymentsResultPage_1?
                .Where(pr => pr.TargetFriend == null)
                .ToList().Should().BeNullOrEmpty();

            okRequestPage_2.Should().BeOfType<OkObjectResult>();
            paymentsResultPage_2.Should().NotBeNullOrEmpty();
            paymentsResultPage_2?.Count().Should().Be(1);
            paymentsResultPage_2?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Status))
                .ToList().Should().BeNullOrEmpty();
            paymentsResultPage_2?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName))
                .ToList().Should().BeNullOrEmpty();
            paymentsResultPage_2?
                .Where(pr => pr.Payment.IdSharedPayment != null ||
                          pr.Payment.IdSharedPayment > 0)
                .ToList().Should().NotBeNullOrEmpty();
            paymentsResultPage_2?
                .Where(pr => pr.TargetFriend == null)
                .ToList().Should().BeNullOrEmpty();

            notFoundRequest_2.Should().BeOfType<NotFoundResult>();
            notFoundRequest_3.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_2?.Contains("is not specified").Should().BeTrue();

            badRequest_3.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_3?.Contains("is invalid").Should().BeTrue();

            badRequest_4.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_4?.Contains("is invalid").Should().BeTrue();
        }

        [Fact]
        public async Task PaymentsController_GetPaymentsSharedWithUserAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var paymentApiService = new PaymentApiService(await dbContext);
            var paymentsController = new PaymentsController(paymentApiService, budgetApiService, _mapper);
            var pages = new { page0 = 0, page1 = 1, page2 = 2 };

            //Act
            var okRequest_1 = await paymentsController.GetPaymentsSharedWithUserAsync(_budgetDate, _friend_1);
            var paymentsResult_1 = (okRequest_1 as OkObjectResult)?.Value as IEnumerable<GetAssignedPaymentToUserDto>;

            var okRequest_2 = await paymentsController.GetPaymentsSharedWithUserAsync(_budgetDate, _friend_2);
            var paymentsResult_2 = (okRequest_2 as OkObjectResult)?.Value as IEnumerable<GetAssignedPaymentToUserDto>;

            var okRequestPage_1 = await paymentsController.GetPaymentsSharedWithUserAsync
                (_budgetDate, _friend_2, pages.page1, _defaultPageSize);
            var paymentsResultPage_1 = (okRequestPage_1 as OkObjectResult)?.Value as IEnumerable<GetAssignedPaymentToUserDto>;

            var okRequestPage_2 = await paymentsController.GetPaymentsSharedWithUserAsync
                (_budgetDate, _friend_2, pages.page2, _defaultPageSize);
            var paymentsResultPage_2 = (okRequestPage_2 as OkObjectResult)?.Value as IEnumerable<GetAssignedPaymentToUserDto>;

            var notFoundRequest_2 = await paymentsController.GetPaymentsSharedWithUserAsync(_budgetDate, _friend_3);
            var notFoundRequest_3 = await paymentsController.GetPaymentsSharedWithUserAsync(_budgetDate, _friend_4);

            var badRequest_1 = await paymentsController.GetPaymentsSharedWithUserAsync(_budgetDate, -_friend_1);
            var badRequestResult_1 = (badRequest_1 as BadRequestObjectResult)?.Value as string;

            var badRequest_2 = await paymentsController.GetPaymentsSharedWithUserAsync(new DateTime(), _friend_1);
            var badRequestResult_2 = (badRequest_2 as BadRequestObjectResult)?.Value as string;

            var badRequest_3 = await paymentsController.GetPaymentsSharedWithUserAsync
                (_budgetDate, _friend_1, pages.page0, _defaultPageSize);
            var badRequestResult_3 = (badRequest_3 as BadRequestObjectResult)?.Value as string;

            var badRequest_4 = await paymentsController.GetPaymentsSharedWithUserAsync
                (_budgetDate, _friend_1, pages.page1, -_defaultPageSize);
            var badRequestResult_4 = (badRequest_4 as BadRequestObjectResult)?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            paymentsResult_1.Should().NotBeNullOrEmpty();
            paymentsResult_1?.Count().Should().Be(1);
            paymentsResult_1?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(pr => pr.Payment.IdSharedPayment <= 0
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(pr => pr.Payment.PaymentDate != null &&
                    (pr.Payment.PaymentDate <= DateTime.Parse("2024-01-01")
                    && pr.Payment.PaymentDate >= DateTime.Parse("2024-01-31"))
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(pr => pr.SourceFriend == null
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(pr => pr.SourceFriend.FriendTag == 1001)
                .ToList().Count().Should().Be(0);

            okRequest_2.Should().BeOfType<OkObjectResult>();
            paymentsResult_2.Should().NotBeNullOrEmpty();
            paymentsResult_2?.Count().Should().Be(2);
            paymentsResult_2?
                .Where( pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2?
                .Where(pr => pr.Payment.IdSharedPayment <= 0
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2?
                .Where(pr => pr.Payment.PaymentDate != null &&
                    (pr.Payment.PaymentDate <= DateTime.Parse("2024-01-01")
                    && pr.Payment.PaymentDate >= DateTime.Parse("2024-01-31"))
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2?
                .Where(pr => pr.SourceFriend == null
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2?
                .Where(pr => pr.SourceFriend.FriendTag == 1002)
                .ToList().Count().Should().Be(0);

            okRequestPage_1.Should().BeOfType<OkObjectResult>();
            paymentsResultPage_1.Should().NotBeNullOrEmpty();
            paymentsResultPage_1?.Count().Should().Be(1);
            paymentsResultPage_1?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResultPage_1?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResultPage_1?
                .Where(pr => pr.Payment.IdSharedPayment <= 0
                ).ToList().Should().BeNullOrEmpty();
            paymentsResultPage_1?
                .Where(pr => pr.Payment.PaymentDate != null &&
                    (pr.Payment.PaymentDate <= DateTime.Parse("2024-01-01")
                    && pr.Payment.PaymentDate >= DateTime.Parse("2024-01-31"))
                ).ToList().Should().BeNullOrEmpty();
            paymentsResultPage_1?
                .Where(pr => pr.SourceFriend == null
                ).ToList().Should().BeNullOrEmpty();
            paymentsResultPage_1?
                .Where(pr => pr.SourceFriend.FriendTag == 1002)
                .ToList().Count().Should().Be(0);

            okRequestPage_2.Should().BeOfType<OkObjectResult>();
            paymentsResultPage_2.Should().NotBeNullOrEmpty();
            paymentsResultPage_2?.Count().Should().Be(1);
            paymentsResultPage_2?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResultPage_2?
                .Where(pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResultPage_2?
                .Where(pr => pr.Payment.IdSharedPayment <= 0
                ).ToList().Should().BeNullOrEmpty();
            paymentsResultPage_2?
                .Where(pr => pr.Payment.PaymentDate != null &&
                    (pr.Payment.PaymentDate <= DateTime.Parse("2024-01-01")
                    && pr.Payment.PaymentDate >= DateTime.Parse("2024-01-31"))
                ).ToList().Should().BeNullOrEmpty();
            paymentsResultPage_2?
                .Where(pr => pr.SourceFriend == null
                ).ToList().Should().BeNullOrEmpty();
            paymentsResultPage_2?
                .Where(pr => pr.SourceFriend.FriendTag == 1002)
                .ToList().Count().Should().Be(0);

            notFoundRequest_2.Should().BeOfType<NotFoundResult>();
            notFoundRequest_3.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_2?.Contains("is not specified").Should().BeTrue();

            badRequest_3.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_3?.Contains("is invalid").Should().BeTrue();

            badRequest_4.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_4?.Contains("is invalid").Should().BeTrue();
        }

        [Fact]
        public async Task PaymentsController_GetDefaultGeneralCategoriesAsync_ReturnsCategoriesOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var paymentApiService = new PaymentApiService(await dbContext);
            var paymentsController = new PaymentsController(paymentApiService, budgetApiService, _mapper);

            //Act
            var okRequest_1 = await paymentsController.GetDefaultGeneralCategoriesAsync();
            var generalCategoriessResult_1 = (okRequest_1 as OkObjectResult)?.Value as IEnumerable<GetGeneralCategoryDto>;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            generalCategoriessResult_1.Should().NotBeNullOrEmpty();
            generalCategoriessResult_1?.Count().Should().Be(3);
            generalCategoriessResult_1?
                .Where(gcr => string.IsNullOrEmpty(gcr.Name))
                .Should().BeNullOrEmpty();
            generalCategoriessResult_1?
                .Where(gcr => gcr.IdGeneralCategory <= 0)
                .Should().BeNullOrEmpty();
            generalCategoriessResult_1?
                .Where(gcr => gcr.IsDefault)
                .Count().Should().Be(3);
        }
    }
}
