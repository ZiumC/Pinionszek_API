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
    }
}
