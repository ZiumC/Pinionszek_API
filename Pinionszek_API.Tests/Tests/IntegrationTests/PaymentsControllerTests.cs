using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pinionszek_API.Controllers;
using Pinionszek_API.Models.DTOs.GetDto.Payments;
using Pinionszek_API.Profiles;
using Pinionszek_API.Services.DatabaseServices.BudgetService;
using Pinionszek_API.Services.DatabaseServices.PaymentService;
using Pinionszek_API.Tests.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinionszek_API.Tests.Tests.IntegrationTests
{
    public class PaymentsControllerTests
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public PaymentsControllerTests()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PaymentProfile());
                cfg.AddProfile(new CategoryProfile());
                cfg.AddProfile(new BudgetProfile());
            });
            _mapper = mockMapper.CreateMapper();

            _config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(
                 path: "appsettings.json",
                 optional: false,
                 reloadOnChange: true)
           .Build();
        }

        [Fact]
        public async Task PaymentsController_GetUpcomingPrivatePaymentsAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var paymentApiService = new PaymentApiService(await dbContext);
            var paymentsController = new PaymentsController(paymentApiService, budgetApiService, _mapper);
            var budgetDate = DateTime.Parse("2024-01-01");
            var user_1 = 1;
            var user_2 = 2;
            var user_3 = 3;
            var user_4 = 4;
            var user_5 = 1001;

            //Act
            var okRequest_1 = await paymentsController.GetUpcomingPrivatePaymentsAsync(budgetDate, user_1, 1, 1);
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var paymentsResult_1 = okActionResult_1?.Value as IEnumerable<GetPrivatePaymentDto>;

            var okRequest_2 = await paymentsController.GetUpcomingPrivatePaymentsAsync(budgetDate, user_2);
            var okActionResult_2 = okRequest_2 as OkObjectResult;
            var paymentsResult_2 = okActionResult_2?.Value as IEnumerable<GetPrivatePaymentDto>;

            var okRequest_3_page_1 = await paymentsController.GetUpcomingPrivatePaymentsAsync(budgetDate, user_2, 1, 1);
            var okActionResult_3_page_1 = okRequest_3_page_1 as OkObjectResult;
            var paymentsResult_3_page_1 = okActionResult_3_page_1?.Value as IEnumerable<GetPrivatePaymentDto>;

            var okRequest_3_page_2 = await paymentsController.GetUpcomingPrivatePaymentsAsync(budgetDate, user_2, 2, 1);
            var okActionResult_3_page_2 = okRequest_3_page_2 as OkObjectResult;
            var paymentsResult_3_page_2 = okActionResult_3_page_2?.Value as IEnumerable<GetPrivatePaymentDto>;

            var notFoundRequest_1 = await paymentsController.GetUpcomingPrivatePaymentsAsync(budgetDate, user_3);
            var notFoundRequest_2 = await paymentsController.GetUpcomingPrivatePaymentsAsync(budgetDate, user_4);

            var badRequest_1 = await paymentsController.GetUpcomingPrivatePaymentsAsync(budgetDate, -user_5);
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

            var badRequest_2 = await paymentsController.GetUpcomingPrivatePaymentsAsync(new DateTime(), user_5);
            var badRequestActionResult_2 = badRequest_2 as BadRequestObjectResult;
            var badRequestResult_2 = badRequestActionResult_2?.Value as string;

            var badRequest_3 = await paymentsController.GetUpcomingPrivatePaymentsAsync(budgetDate, user_1, 0, 10);
            var badRequestActionResult_3 = badRequest_3 as BadRequestObjectResult;
            var badRequestResult_3 = badRequestActionResult_3?.Value as string;

            var badRequest_4 = await paymentsController.GetUpcomingPrivatePaymentsAsync(budgetDate, user_1, 1, -10);
            var badRequestActionResult_4 = badRequest_4 as BadRequestObjectResult;
            var badRequestResult_4 = badRequestActionResult_4?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            okActionResult_1.Should().NotBeNull();
            paymentsResult_1.Should().NotBeNullOrEmpty();
            paymentsResult_1?.Count().Should().Be(1);
            paymentsResult_1?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Status)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Category.GeneralName)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(
                    pr => pr.IdSharedPayment != null ||
                          pr.IdSharedPayment > 0
                ).ToList()
                .Should().BeNullOrEmpty();


            okRequest_2.Should().BeOfType<OkObjectResult>();
            okActionResult_2.Should().NotBeNull();
            paymentsResult_2.Should().NotBeNullOrEmpty();
            paymentsResult_2?.Count().Should().Be(2);
            paymentsResult_2?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Status)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_2?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Category.GeneralName)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_2?
                .Where(
                    pr => pr.IdSharedPayment != null ||
                          pr.IdSharedPayment > 0
                ).ToList()
                .Should().BeNullOrEmpty();

            okRequest_3_page_1.Should().BeOfType<OkObjectResult>();
            okActionResult_3_page_1.Should().NotBeNull();
            paymentsResult_3_page_1.Should().NotBeNullOrEmpty();
            paymentsResult_3_page_1?.Count().Should().Be(1);
            paymentsResult_3_page_1?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Status)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_3_page_1?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Category.GeneralName)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_3_page_1?
                .Where(
                    pr => pr.IdSharedPayment != null ||
                          pr.IdSharedPayment > 0
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_3_page_1?
                .Where(pr => pr.IdPayment == 13)
                .ToList().Count()
                .Should().Be(1);

            okRequest_3_page_2.Should().BeOfType<OkObjectResult>();
            okActionResult_3_page_2.Should().NotBeNull();
            paymentsResult_3_page_2.Should().NotBeNullOrEmpty();
            paymentsResult_3_page_2?.Count().Should().Be(1);
            paymentsResult_3_page_2?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Status)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_3_page_2?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Category.GeneralName)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_3_page_2?
                .Where(
                    pr => pr.IdSharedPayment != null ||
                          pr.IdSharedPayment > 0
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_3_page_2?
                .Where(pr => pr.IdPayment == 7)
                .ToList().Count()
                .Should().Be(1);

            notFoundRequest_1.Should().BeOfType<NotFoundResult>();
            notFoundRequest_2.Should().BeOfType<NotFoundResult>();


            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_1?.Value.Should().NotBeNull();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_2?.Value.Should().NotBeNull();
            badRequestResult_2?.Contains("is not specified").Should().BeTrue();

            badRequest_3.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_3?.Value.Should().NotBeNull();
            badRequestResult_3?.Contains("is is invalid").Should().BeTrue();

            badRequest_4.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_4?.Value.Should().NotBeNull();
            badRequestResult_4?.Contains("is is invalid").Should().BeTrue();
        }

        [Fact]
        public async Task PaymentsController_GetUpcomingPaymentsSharedWithFriendAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var paymentsApiService = new PaymentApiService(await dbContext);
            var paymentsController = new PaymentsController(paymentsApiService, budgetApiService, _mapper);
            var budgetDate = DateTime.Parse("2024-01-01");
            var user_1 = 1;
            var user_2 = 2;
            var user_3 = 3;
            var user_4 = 4;
            var user_5 = 1001;

            //Act
            var okRequest_1 = await paymentsController.GetUpcomingPaymentsSharedWithFriendAsync(budgetDate, user_1);
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var paymentsResult_1 = okActionResult_1?.Value as IEnumerable<GetSharedPaymentToFriendDto>;

            var okRequest_2_page_1 = await paymentsController.GetUpcomingPaymentsSharedWithFriendAsync(budgetDate, user_1, 1, 1);
            var okActionResult_2_page_1 = okRequest_2_page_1 as OkObjectResult;
            var paymentsResult_2_page_1 = okActionResult_2_page_1?.Value as IEnumerable<GetSharedPaymentToFriendDto>;

            var okRequest_2_page_2 = await paymentsController.GetUpcomingPaymentsSharedWithFriendAsync(budgetDate, user_1, 2, 1);
            var okActionResult_2_page_2 = okRequest_2_page_2 as OkObjectResult;
            var paymentsResult_2_page_2 = okActionResult_2_page_2?.Value as IEnumerable<GetSharedPaymentToFriendDto>;

            var notFoundRequest_1 = await paymentsController.GetUpcomingPaymentsSharedWithFriendAsync(budgetDate, user_2);
            var notFoundRequest_2 = await paymentsController.GetUpcomingPaymentsSharedWithFriendAsync(budgetDate, user_3);
            var notFoundRequest_3 = await paymentsController.GetUpcomingPaymentsSharedWithFriendAsync(budgetDate, user_4);

            var badRequest_1 = await paymentsController.GetUpcomingPaymentsSharedWithFriendAsync(budgetDate, -user_5);
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

            var badRequest_2 = await paymentsController.GetUpcomingPaymentsSharedWithFriendAsync(new DateTime(), user_5);
            var badRequestActionResult_2 = badRequest_2 as BadRequestObjectResult;
            var badRequestResult_2 = badRequestActionResult_2?.Value as string;

            var badRequest_3 = await paymentsController.GetUpcomingPaymentsSharedWithFriendAsync(budgetDate, user_1, -1, 1);
            var badRequestActionResult_3 = badRequest_3 as BadRequestObjectResult;
            var badRequestResult_3 = badRequestActionResult_3?.Value as string;

            var badRequest_4 = await paymentsController.GetUpcomingPaymentsSharedWithFriendAsync(budgetDate, user_1, -1, 1);
            var badRequestActionResult_4 = badRequest_4 as BadRequestObjectResult;
            var badRequestResult_4 = badRequestActionResult_4?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            okActionResult_1.Should().NotBeNull();
            paymentsResult_1.Should().NotBeNullOrEmpty();
            paymentsResult_1?.Count().Should().Be(2);
            paymentsResult_1?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(
                    pr => pr.Payment.IdSharedPayment != null ||
                          pr.Payment.IdSharedPayment > 0
                ).ToList()
                .Should().NotBeNullOrEmpty();
            paymentsResult_1?
                .Where(
                    pr => pr.TargetFriend == null
                ).ToList()
                .Should().BeNullOrEmpty();

            okRequest_2_page_1.Should().BeOfType<OkObjectResult>();
            okActionResult_2_page_1.Should().NotBeNull();
            paymentsResult_2_page_1.Should().NotBeNullOrEmpty();
            paymentsResult_2_page_1?.Count().Should().Be(1);
            paymentsResult_2_page_1?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_2_page_1?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_2_page_1?
                .Where(
                    pr => pr.Payment.IdSharedPayment != null ||
                          pr.Payment.IdSharedPayment > 0
                ).ToList()
                .Should().NotBeNullOrEmpty();
            paymentsResult_2_page_1?
                .Where(
                    pr => pr.TargetFriend == null
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_2_page_1?
                .Where(pr => pr.Payment.IdPayment == 4)
                .ToList().Count()
                .Should().Be(1);

            okRequest_2_page_2.Should().BeOfType<OkObjectResult>();
            okActionResult_2_page_2.Should().NotBeNull();
            paymentsResult_2_page_2.Should().NotBeNullOrEmpty();
            paymentsResult_2_page_2?.Count().Should().Be(1);
            paymentsResult_2_page_2?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_2_page_2?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_2_page_2?
                .Where(
                    pr => pr.Payment.IdSharedPayment != null ||
                          pr.Payment.IdSharedPayment > 0
                ).ToList()
                .Should().NotBeNullOrEmpty();
            paymentsResult_2_page_2?
                .Where(
                    pr => pr.TargetFriend == null
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_2_page_2?
                .Where(pr => pr.Payment.IdPayment == 1)
                .ToList().Count()
                .Should().Be(1);


            notFoundRequest_1.Should().BeOfType<NotFoundResult>();
            notFoundRequest_2.Should().BeOfType<NotFoundResult>();
            notFoundRequest_3.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_1?.Value.Should().NotBeNull();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_2?.Value.Should().NotBeNull();
            badRequestResult_2?.Contains("is not specified").Should().BeTrue();

            badRequest_3.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_3?.Value.Should().NotBeNull();
            badRequestResult_3?.Contains("is invalid").Should().BeTrue();

            badRequest_4.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_4?.Value.Should().NotBeNull();
            badRequestResult_4?.Contains("is invalid").Should().BeTrue();
        }

        [Fact]
        public async Task PaymentsController_GetUpcomingPaymentsSharedWithUserAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var paymentsApiService = new PaymentApiService(await dbContext);
            var paymentsController = new PaymentsController(paymentsApiService, budgetApiService, _mapper);
            var budgetDate = DateTime.Parse("2024-01-01");
            var friend_1 = 1001;
            var friend_2 = 1002;
            var friend_3 = 1003;
            var friend_4 = 1004;

            //Act
            var okRequest_1 = await paymentsController.GetUpcomingPaymentsSharedWithUserAsync(budgetDate, friend_2);
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var paymentsResult_1 = okActionResult_1?.Value as IEnumerable<GetAssignedPaymentToUserDto>;

            var okRequest_2_page_1 = await paymentsController.GetUpcomingPaymentsSharedWithUserAsync(budgetDate, friend_2, 1, 1);
            var okActionResult_2_page_1 = okRequest_2_page_1 as OkObjectResult;
            var paymentsResult_2_page_1 = okActionResult_2_page_1?.Value as IEnumerable<GetAssignedPaymentToUserDto>;

            var okRequest_2_page_2 = await paymentsController.GetUpcomingPaymentsSharedWithUserAsync(budgetDate, friend_2, 2, 1);
            var okActionResult_2_page_2 = okRequest_2_page_2 as OkObjectResult;
            var paymentsResult_2_page_2 = okActionResult_2_page_2?.Value as IEnumerable<GetAssignedPaymentToUserDto>;

            var notFoundRequest_1 = await paymentsController.GetUpcomingPaymentsSharedWithUserAsync(budgetDate, friend_1);
            var notFoundRequest_2 = await paymentsController.GetUpcomingPaymentsSharedWithUserAsync(budgetDate, friend_3);
            var notFoundRequest_3 = await paymentsController.GetUpcomingPaymentsSharedWithUserAsync(budgetDate, friend_4);

            var badRequest_1 = await paymentsController.GetUpcomingPaymentsSharedWithUserAsync(budgetDate, -friend_1);
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

            var badRequest_2 = await paymentsController.GetUpcomingPaymentsSharedWithUserAsync(new DateTime(), friend_1);
            var badRequestActionResult_2 = badRequest_2 as BadRequestObjectResult;
            var badRequestResult_2 = badRequestActionResult_2?.Value as string;

            var badRequest_3 = await paymentsController.GetUpcomingPaymentsSharedWithUserAsync(budgetDate, friend_2, -1, 10);
            var badRequestActionResult_3 = badRequest_3 as BadRequestObjectResult;
            var badRequestResult_3 = badRequestActionResult_3?.Value as string;

            var badRequest_4 = await paymentsController.GetUpcomingPaymentsSharedWithUserAsync(budgetDate, friend_2, 1, -10);
            var badRequestActionResult_4 = badRequest_4 as BadRequestObjectResult;
            var badRequestResult_4 = badRequestActionResult_4?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            okActionResult_1.Should().NotBeNull();
            paymentsResult_1.Should().NotBeNullOrEmpty();
            paymentsResult_1?.Count().Should().Be(2);
            paymentsResult_1?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(
                    pr => pr.Payment.IdSharedPayment <= 0
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(
                    pr => pr.Payment.PaymentDate != null &&
                    (pr.Payment.PaymentDate <= DateTime.Parse("2024-01-01")
                    && pr.Payment.PaymentDate >= DateTime.Parse("2024-01-31"))
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(
                    pr => pr.SourceFriend == null
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(pr => pr.SourceFriend.FriendTag == 1002)
                .ToList().Count().Should().Be(0);

            okRequest_2_page_1.Should().BeOfType<OkObjectResult>();
            okActionResult_2_page_1.Should().NotBeNull();
            paymentsResult_2_page_1.Should().NotBeNullOrEmpty();
            paymentsResult_2_page_1?.Count().Should().Be(1);
            paymentsResult_2_page_1?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2_page_1?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2_page_1?
                .Where(
                    pr => pr.Payment.IdSharedPayment <= 0
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2_page_1?
                .Where(
                    pr => pr.Payment.PaymentDate != null &&
                    (pr.Payment.PaymentDate <= DateTime.Parse("2024-01-01")
                    && pr.Payment.PaymentDate >= DateTime.Parse("2024-01-31"))
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2_page_1?
                .Where(
                    pr => pr.SourceFriend == null
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2_page_1?
                .Where(pr => pr.SourceFriend.FriendTag == 1002)
                .ToList().Count().Should().Be(0);
            paymentsResult_2_page_1?
                .Where(pr => pr.Payment.IdPayment == 4)
                .ToList().Count().Should().Be(1);

            okRequest_2_page_2.Should().BeOfType<OkObjectResult>();
            okActionResult_2_page_2.Should().NotBeNull();
            paymentsResult_2_page_2.Should().NotBeNullOrEmpty();
            paymentsResult_2_page_2?.Count().Should().Be(1);
            paymentsResult_2_page_2?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2_page_2?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2_page_2?
                .Where(
                    pr => pr.Payment.IdSharedPayment <= 0
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2_page_2?
                .Where(
                    pr => pr.Payment.PaymentDate != null &&
                    (pr.Payment.PaymentDate <= DateTime.Parse("2024-01-01")
                    && pr.Payment.PaymentDate >= DateTime.Parse("2024-01-31"))
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2_page_2?
                .Where(
                    pr => pr.SourceFriend == null
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
            badRequestActionResult_1?.Value.Should().NotBeNull();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_2?.Value.Should().NotBeNull();
            badRequestResult_2?.Contains("is not specified").Should().BeTrue();

            badRequest_3.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_3?.Value.Should().NotBeNull();
            badRequestResult_3?.Contains("is invalid").Should().BeTrue();

            badRequest_4.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_4?.Value.Should().NotBeNull();
            badRequestResult_4?.Contains("is invalid").Should().BeTrue();
        }

        [Fact]
        public async Task PaymentsController_GetPaymentDetailsAsync_ReturnsPaymentOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var paymentsApiService = new PaymentApiService(await dbContext);
            var paymentsController = new PaymentsController(paymentsApiService, budgetApiService, _mapper);
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
            var paymentsApiService = new PaymentApiService(await dbContext);
            var budgetController = new PaymentsController(paymentsApiService, budgetApiService, _mapper);
            var budgetDate = DateTime.Parse("2024-01-01");
            var user_1 = 1;
            var user_2 = 2;
            var user_3 = 3;
            var user_4 = 1005;

            //Act
            var okRequest_1 = await budgetController.GetPrivatePaymentsAsync(budgetDate, user_1);
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var paymentsResult_1 = okActionResult_1?.Value as List<GetPrivatePaymentDto>;

            var okRequest_2 = await budgetController.GetPrivatePaymentsAsync(budgetDate, user_2);
            var okActionResult_2 = okRequest_2 as OkObjectResult;
            var paymentsResult_2 = okActionResult_2?.Value as List<GetPrivatePaymentDto>;

            var okRequest_3_page_1 = await budgetController.GetPrivatePaymentsAsync(budgetDate, user_1, 1, 3);
            var okActionResult_3_page_1 = okRequest_3_page_1 as OkObjectResult;
            var paymentsResult_3_page_1 = okActionResult_3_page_1?.Value as List<GetPrivatePaymentDto>;

            var okRequest_3_page_2 = await budgetController.GetPrivatePaymentsAsync(budgetDate, user_1, 2, 3);
            var okActionResult_3_page_2 = okRequest_3_page_2 as OkObjectResult;
            var paymentsResult_3_page_2 = okActionResult_3_page_2?.Value as List<GetPrivatePaymentDto>;

            var notfoundRequest_1 = await budgetController.GetPrivatePaymentsAsync(budgetDate, user_3);

            var badRequest_1 = await budgetController.GetPrivatePaymentsAsync(budgetDate, -user_4);
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

            var badRequest_2 = await budgetController.GetPrivatePaymentsAsync(new DateTime(), user_1);
            var badRequestActionResult_2 = badRequest_2 as BadRequestObjectResult;
            var badRequestResult_2 = badRequestActionResult_2?.Value as string;

            var badRequest_3 = await budgetController.GetPrivatePaymentsAsync(budgetDate, user_1, -1, 1);
            var badRequestActionResult_3 = badRequest_3 as BadRequestObjectResult;
            var badRequestResult_3 = badRequestActionResult_3?.Value as string;

            var badRequest_4 = await budgetController.GetPrivatePaymentsAsync(budgetDate, user_1, 1, 0);
            var badRequestActionResult_4 = badRequest_4 as BadRequestObjectResult;
            var badRequestResult_4 = badRequestActionResult_4?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            okActionResult_1.Should().NotBeNull();
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
            okActionResult_2.Should().NotBeNull();
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

            okRequest_3_page_1.Should().BeOfType<OkObjectResult>();
            okActionResult_3_page_1.Should().NotBeNull();
            paymentsResult_3_page_1.Should().NotBeNull();
            paymentsResult_3_page_1.Should().BeOfType<List<GetPrivatePaymentDto>>();
            paymentsResult_3_page_1?.Count().Should().Be(3);
            paymentsResult_3_page_1?
                .Where(pr => pr.IdPayment <= 0)
                .Should().BeEmpty();
            paymentsResult_3_page_1?
                .Where(pr => string.IsNullOrEmpty(pr.Name))
                .Should().BeEmpty();
            paymentsResult_3_page_1?
                .Where(pr => pr.Charge >= 0 || pr.Refund >= 0)
                .Should().NotBeEmpty();
            paymentsResult_3_page_1?
                .Where(pr => string.IsNullOrEmpty(pr.Status))
                .Should().BeEmpty();
            paymentsResult_3_page_1?
                .Where(pr => pr.Category == null ||
                string.IsNullOrEmpty(pr.Category.DetailedName) ||
                string.IsNullOrEmpty(pr.Category.GeneralName))
                .Should().BeEmpty();

            okRequest_3_page_2.Should().BeOfType<OkObjectResult>();
            okActionResult_3_page_2.Should().NotBeNull();
            paymentsResult_3_page_2.Should().NotBeNull();
            paymentsResult_3_page_2.Should().BeOfType<List<GetPrivatePaymentDto>>();
            paymentsResult_3_page_2?.Count().Should().Be(2);
            paymentsResult_3_page_2?
                .Where(pr => pr.IdPayment <= 0)
                .Should().BeEmpty();
            paymentsResult_3_page_2?
                .Where(pr => string.IsNullOrEmpty(pr.Name))
                .Should().BeEmpty();
            paymentsResult_3_page_2?
                .Where(pr => pr.Charge >= 0 || pr.Refund >= 0)
                .Should().NotBeEmpty();
            paymentsResult_3_page_2?
                .Where(pr => string.IsNullOrEmpty(pr.Status))
                .Should().BeEmpty();
            paymentsResult_3_page_2?
                .Where(pr => pr.Category == null ||
                string.IsNullOrEmpty(pr.Category.DetailedName) ||
                string.IsNullOrEmpty(pr.Category.GeneralName))
                .Should().BeEmpty();

            notfoundRequest_1.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_1?.Value.Should().NotBeNull();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_2?.Value.Should().NotBeNull();
            badRequestResult_2?.Contains("is not specified").Should().BeTrue();

            badRequest_3.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_3?.Value.Should().NotBeNull();
            badRequestResult_3?.Contains("is invalid").Should().BeTrue();

            badRequest_4.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_4?.Value.Should().NotBeNull();
            badRequestResult_4?.Contains("is invalid").Should().BeTrue();
        }

        [Fact]
        public async Task PaymentsController_GetPaymentsSharedWithFriendAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var paymentsApiService = new PaymentApiService(await dbContext);
            var paymentsController = new PaymentsController(paymentsApiService, budgetApiService, _mapper);
            var budgetDate = DateTime.Parse("2024-01-01");
            var user_1 = 1;
            var user_2 = 2;
            var user_3 = 3;
            var user_4 = 4;
            var user_5 = 1005;

            //Act
            var okRequest_1 = await paymentsController.GetPaymentsSharedWithFriendAsync(budgetDate, user_1);
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var paymentsResult_1 = okActionResult_1?.Value as IEnumerable<GetSharedPaymentToFriendDto>;

            var okRequest_2 = await paymentsController.GetPaymentsSharedWithFriendAsync(budgetDate, user_2);
            var okActionResult_2 = okRequest_2 as OkObjectResult;
            var paymentsResult_2 = okActionResult_2?.Value as IEnumerable<GetSharedPaymentToFriendDto>;

            var okRequest_3_page_1 = await paymentsController.GetPaymentsSharedWithFriendAsync(budgetDate, user_1, 1, 1);
            var okActionResult_3_page_1 = okRequest_3_page_1 as OkObjectResult;
            var paymentsResult_3_page_1 = okActionResult_3_page_1?.Value as IEnumerable<GetSharedPaymentToFriendDto>;

            var okRequest_3_page_2 = await paymentsController.GetPaymentsSharedWithFriendAsync(budgetDate, user_1, 2, 1);
            var okActionResult_3_page_2 = okRequest_3_page_2 as OkObjectResult;
            var paymentsResult_3_page_2 = okActionResult_3_page_2?.Value as IEnumerable<GetSharedPaymentToFriendDto>;

            var notFoundRequest_2 = await paymentsController.GetPaymentsSharedWithFriendAsync(budgetDate, user_3);
            var notFoundRequest_3 = await paymentsController.GetPaymentsSharedWithFriendAsync(budgetDate, user_4);

            var badRequest_1 = await paymentsController.GetPaymentsSharedWithFriendAsync(budgetDate, -user_5);
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

            var badRequest_2 = await paymentsController.GetPaymentsSharedWithFriendAsync(new DateTime(), user_5);
            var badRequestActionResult_2 = badRequest_2 as BadRequestObjectResult;
            var badRequestResult_2 = badRequestActionResult_2?.Value as string;

            var badRequest_3 = await paymentsController.GetPaymentsSharedWithFriendAsync(budgetDate, user_1, 0, 1);
            var badRequestActionResult_3 = badRequest_3 as BadRequestObjectResult;
            var badRequestResult_3 = badRequestActionResult_3?.Value as string;

            var badRequest_4 = await paymentsController.GetPaymentsSharedWithFriendAsync(budgetDate, user_1, 1, -1);
            var badRequestActionResult_4 = badRequest_4 as BadRequestObjectResult;
            var badRequestResult_4 = badRequestActionResult_4?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            okActionResult_1.Should().NotBeNull();
            paymentsResult_1.Should().NotBeNullOrEmpty();
            paymentsResult_1?.Count().Should().Be(2);
            paymentsResult_1?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(
                    pr => pr.Payment.IdSharedPayment != null ||
                          pr.Payment.IdSharedPayment > 0
                ).ToList()
                .Should().NotBeNullOrEmpty();
            paymentsResult_1?
                .Where(
                    pr => pr.TargetFriend == null
                ).ToList()
                .Should().BeNullOrEmpty();

            okRequest_2.Should().BeOfType<OkObjectResult>();
            okActionResult_2.Should().NotBeNull();
            paymentsResult_2.Should().NotBeNullOrEmpty();
            paymentsResult_2?.Count().Should().Be(1);
            paymentsResult_2?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_2?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_2?
                .Where(
                    pr => pr.Payment.IdSharedPayment != null ||
                          pr.Payment.IdSharedPayment > 0
                ).ToList()
                .Should().NotBeNullOrEmpty();
            paymentsResult_2?
                .Where(
                    pr => pr.TargetFriend == null
                ).ToList()
                .Should().BeNullOrEmpty();

            okRequest_3_page_1.Should().BeOfType<OkObjectResult>();
            okActionResult_3_page_1.Should().NotBeNull();
            paymentsResult_3_page_1.Should().NotBeNullOrEmpty();
            paymentsResult_3_page_1?.Count().Should().Be(1);
            paymentsResult_3_page_1?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_3_page_1?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_3_page_1?
                .Where(
                    pr => pr.Payment.IdSharedPayment != null ||
                          pr.Payment.IdSharedPayment > 0
                ).ToList()
                .Should().NotBeNullOrEmpty();
            paymentsResult_3_page_1?
                .Where(
                    pr => pr.TargetFriend == null
                ).ToList()
                .Should().BeNullOrEmpty();

            okRequest_3_page_2.Should().BeOfType<OkObjectResult>();
            okActionResult_3_page_2.Should().NotBeNull();
            paymentsResult_3_page_2.Should().NotBeNullOrEmpty();
            paymentsResult_3_page_2?.Count().Should().Be(1);
            paymentsResult_3_page_2?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_3_page_2?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList()
                .Should().BeNullOrEmpty();
            paymentsResult_3_page_2?
                .Where(
                    pr => pr.Payment.IdSharedPayment != null ||
                          pr.Payment.IdSharedPayment > 0
                ).ToList()
                .Should().NotBeNullOrEmpty();
            paymentsResult_3_page_2?
                .Where(
                    pr => pr.TargetFriend == null
                ).ToList()
                .Should().BeNullOrEmpty();

            notFoundRequest_2.Should().BeOfType<NotFoundResult>();
            notFoundRequest_3.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_1?.Value.Should().NotBeNull();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_2?.Value.Should().NotBeNull();
            badRequestResult_2?.Contains("is not specified").Should().BeTrue();

            badRequest_3.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_3?.Value.Should().NotBeNull();
            badRequestResult_3?.Contains("is invalid").Should().BeTrue();

            badRequest_4.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_4?.Value.Should().NotBeNull();
            badRequestResult_4?.Contains("is invalid").Should().BeTrue();
        }

        [Fact]
        public async Task PaymentsController_GetPaymentsSharedWithUserAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var paymentsApiService = new PaymentApiService(await dbContext);
            var paymentsController = new PaymentsController(paymentsApiService, budgetApiService, _mapper);
            var budgetDate = DateTime.Parse("2024-01-01");
            var friend_1 = 1001;
            var friend_2 = 1002;
            var friend_3 = 1003;
            var friend_4 = 1004;

            //Act
            var okRequest_1 = await paymentsController.GetPaymentsSharedWithUserAsync(budgetDate, friend_1);
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var paymentsResult_1 = okActionResult_1?.Value as IEnumerable<GetAssignedPaymentToUserDto>;

            var okRequest_2 = await paymentsController.GetPaymentsSharedWithUserAsync(budgetDate, friend_2);
            var okActionResult_2 = okRequest_2 as OkObjectResult;
            var paymentsResult_2 = okActionResult_2?.Value as IEnumerable<GetAssignedPaymentToUserDto>;

            var okRequest_3_page_1 = await paymentsController.GetPaymentsSharedWithUserAsync(budgetDate, friend_2, 1, 1);
            var okActionResult_3_page_1 = okRequest_3_page_1 as OkObjectResult;
            var paymentsResult_3_page_1 = okActionResult_3_page_1?.Value as IEnumerable<GetAssignedPaymentToUserDto>;

            var okRequest_3_page_2 = await paymentsController.GetPaymentsSharedWithUserAsync(budgetDate, friend_2, 2, 1);
            var okActionResult_3_page_2 = okRequest_3_page_2 as OkObjectResult;
            var paymentsResult_3_page_2 = okActionResult_3_page_2?.Value as IEnumerable<GetAssignedPaymentToUserDto>;

            var notFoundRequest_2 = await paymentsController.GetPaymentsSharedWithUserAsync(budgetDate, friend_3);
            var notFoundRequest_3 = await paymentsController.GetPaymentsSharedWithUserAsync(budgetDate, friend_4);

            var badRequest_1 = await paymentsController.GetPaymentsSharedWithUserAsync(budgetDate, -friend_1);
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

            var badRequest_2 = await paymentsController.GetPaymentsSharedWithUserAsync(new DateTime(), friend_1);
            var badRequestActionResult_2 = badRequest_2 as BadRequestObjectResult;
            var badRequestResult_2 = badRequestActionResult_2?.Value as string;

            var badRequest_3 = await paymentsController.GetPaymentsSharedWithUserAsync(budgetDate, friend_1, 0, 11);
            var badRequestActionResult_3 = badRequest_3 as BadRequestObjectResult;
            var badRequestResult_3 = badRequestActionResult_3?.Value as string;

            var badRequest_4 = await paymentsController.GetPaymentsSharedWithUserAsync(budgetDate, friend_1, 1, -11);
            var badRequestActionResult_4 = badRequest_4 as BadRequestObjectResult;
            var badRequestResult_4 = badRequestActionResult_4?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            okActionResult_1.Should().NotBeNull();
            paymentsResult_1.Should().NotBeNullOrEmpty();
            paymentsResult_1?.Count().Should().Be(1);
            paymentsResult_1?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(
                    pr => pr.Payment.IdSharedPayment <= 0
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(
                    pr => pr.Payment.PaymentDate != null &&
                    (pr.Payment.PaymentDate <= DateTime.Parse("2024-01-01")
                    && pr.Payment.PaymentDate >= DateTime.Parse("2024-01-31"))
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(
                    pr => pr.SourceFriend == null
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_1?
                .Where(pr => pr.SourceFriend.FriendTag == 1001)
                .ToList().Count().Should().Be(0);

            okRequest_2.Should().BeOfType<OkObjectResult>();
            okActionResult_2.Should().NotBeNull();
            paymentsResult_2.Should().NotBeNullOrEmpty();
            paymentsResult_2?.Count().Should().Be(2);
            paymentsResult_2?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2?
                .Where(
                    pr => pr.Payment.IdSharedPayment <= 0
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2?
                .Where(
                    pr => pr.Payment.PaymentDate != null &&
                    (pr.Payment.PaymentDate <= DateTime.Parse("2024-01-01")
                    && pr.Payment.PaymentDate >= DateTime.Parse("2024-01-31"))
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2?
                .Where(
                    pr => pr.SourceFriend == null
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_2?
                .Where(pr => pr.SourceFriend.FriendTag == 1002)
                .ToList().Count().Should().Be(0);

            okRequest_3_page_1.Should().BeOfType<OkObjectResult>();
            okActionResult_3_page_1.Should().NotBeNull();
            paymentsResult_3_page_1.Should().NotBeNullOrEmpty();
            paymentsResult_3_page_1?.Count().Should().Be(1);
            paymentsResult_3_page_1?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_3_page_1?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_3_page_1?
                .Where(
                    pr => pr.Payment.IdSharedPayment <= 0
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_3_page_1?
                .Where(
                    pr => pr.Payment.PaymentDate != null &&
                    (pr.Payment.PaymentDate <= DateTime.Parse("2024-01-01")
                    && pr.Payment.PaymentDate >= DateTime.Parse("2024-01-31"))
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_3_page_1?
                .Where(
                    pr => pr.SourceFriend == null
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_3_page_1?
                .Where(pr => pr.SourceFriend.FriendTag == 1002)
                .ToList().Count().Should().Be(0);

            okRequest_3_page_2.Should().BeOfType<OkObjectResult>();
            okActionResult_3_page_2.Should().NotBeNull();
            paymentsResult_3_page_2.Should().NotBeNullOrEmpty();
            paymentsResult_3_page_2?.Count().Should().Be(1);
            paymentsResult_3_page_2?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Status)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_3_page_2?
                .Where(
                    pr => string.IsNullOrEmpty(pr.Payment.Category.DetailedName) ||
                          string.IsNullOrEmpty(pr.Payment.Category.GeneralName)
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_3_page_2?
                .Where(
                    pr => pr.Payment.IdSharedPayment <= 0
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_3_page_2?
                .Where(
                    pr => pr.Payment.PaymentDate != null &&
                    (pr.Payment.PaymentDate <= DateTime.Parse("2024-01-01")
                    && pr.Payment.PaymentDate >= DateTime.Parse("2024-01-31"))
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_3_page_2?
                .Where(
                    pr => pr.SourceFriend == null
                ).ToList().Should().BeNullOrEmpty();
            paymentsResult_3_page_2?
                .Where(pr => pr.SourceFriend.FriendTag == 1002)
                .ToList().Count().Should().Be(0);

            notFoundRequest_2.Should().BeOfType<NotFoundResult>();
            notFoundRequest_3.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_1?.Value.Should().NotBeNull();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_2?.Value.Should().NotBeNull();
            badRequestResult_2?.Contains("is not specified").Should().BeTrue();

            badRequest_3.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_3?.Value.Should().NotBeNull();
            badRequestResult_3?.Contains("is invalid").Should().BeTrue();

            badRequest_4.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_4?.Value.Should().NotBeNull();
            badRequestResult_4?.Contains("is invalid").Should().BeTrue();
        }
    }
}
