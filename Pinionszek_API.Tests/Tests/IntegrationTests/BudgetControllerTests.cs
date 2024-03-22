using AutoMapper;
using Castle.Components.DictionaryAdapter.Xml;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Moq;
using Newtonsoft.Json;
using Pinionszek_API.Controllers;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDto;
using Pinionszek_API.Models.DTOs.GetDto.Payments;
using Pinionszek_API.Profiles;
using Pinionszek_API.Services.DatabaseServices.BudgetService;
using Pinionszek_API.Tests.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinionszek_API.Tests.Tests.IntegrationTests
{
    public class BudgetControllerTests
    {

        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public BudgetControllerTests()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PaymentProfile());
                cfg.AddProfile(new CategoryProfile());
                cfg.AddProfile(new BudgetProfile());
                cfg.AddProfile(new UserSettingsProfile());
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
        public async Task BudgetController_GetUpcomingPrivatePaymentsAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var budgetController = new BudgetController(_config, budgetApiService, _mapper);
            var budgetDate = DateTime.Parse("2024-01-01");

            //Act
            var okRequest_1 = await budgetController.GetUpcomingPrivatePaymentsAsync(budgetDate, 1);
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var paymentsResult_1 = okActionResult_1?.Value as IEnumerable<GetPrivatePaymentDto>;

            var okRequest_2 = await budgetController.GetUpcomingPrivatePaymentsAsync(budgetDate, 2);
            var okActionResult_2 = okRequest_2 as OkObjectResult;
            var paymentsResult_2 = okActionResult_2?.Value as IEnumerable<GetPrivatePaymentDto>;

            var notFoundRequest_1 = await budgetController.GetUpcomingPrivatePaymentsAsync(budgetDate, 3);
            var notFoundRequest_2 = await budgetController.GetUpcomingPrivatePaymentsAsync(budgetDate, 4);

            var badRequest_1 = await budgetController.GetUpcomingPrivatePaymentsAsync(budgetDate, -10);
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

            var badRequest_2 = await budgetController.GetUpcomingPrivatePaymentsAsync(new DateTime(), 10);
            var badRequestActionResult_2 = badRequest_2 as BadRequestObjectResult;
            var badRequestResult_2 = badRequestActionResult_2?.Value as string;

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


            notFoundRequest_1.Should().BeOfType<NotFoundResult>();
            notFoundRequest_2.Should().BeOfType<NotFoundResult>();


            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_1?.Value.Should().NotBeNull();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_2?.Value.Should().NotBeNull();
            badRequestResult_2?.Contains("is not specified").Should().BeTrue();
        }


        [Fact]
        public async Task BudgetController_GetUpcomingPaymentsSharedWithFriendAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var budgetController = new BudgetController(_config, budgetApiService, _mapper);
            var budgetDate = DateTime.Parse("2024-01-01");

            //Act
            var okRequest_1 = await budgetController.GetUpcomingPaymentsSharedWithFriendAsync(budgetDate, 1);
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var paymentsResult_1 = okActionResult_1?.Value as IEnumerable<GetSharedPaymentToFriendDto>;

            var notFoundRequest_1 = await budgetController.GetUpcomingPaymentsSharedWithFriendAsync(budgetDate, 2);
            var notFoundRequest_2 = await budgetController.GetUpcomingPaymentsSharedWithFriendAsync(budgetDate, 3);
            var notFoundRequest_3 = await budgetController.GetUpcomingPaymentsSharedWithFriendAsync(budgetDate, 4);

            var badRequest_1 = await budgetController.GetUpcomingPaymentsSharedWithFriendAsync(budgetDate, -1001);
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

            var badRequest_2 = await budgetController.GetUpcomingPaymentsSharedWithFriendAsync(new DateTime(), 1001);
            var badRequestActionResult_2 = badRequest_2 as BadRequestObjectResult;
            var badRequestResult_2 = badRequestActionResult_2?.Value as string;

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


            notFoundRequest_1.Should().BeOfType<NotFoundResult>();
            notFoundRequest_2.Should().BeOfType<NotFoundResult>();
            notFoundRequest_3.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_1?.Value.Should().NotBeNull();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_2?.Value.Should().NotBeNull();
            badRequestResult_2?.Contains("is not specified").Should().BeTrue();
        }

        [Fact]
        public async Task BudgetController_GetUpcomingPaymentsSharedWithUserAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var budgetController = new BudgetController(_config, budgetApiService, _mapper);
            var budgetDate = DateTime.Parse("2024-01-01");

            //Act
            var okRequest_1 = await budgetController.GetUpcomingPaymentsSharedWithUserAsync(budgetDate, 1002);
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var paymentsResult_1 = okActionResult_1?.Value as IEnumerable<GetAssignedPaymentToUserDto>;

            var notFoundRequest_1 = await budgetController.GetUpcomingPaymentsSharedWithUserAsync(budgetDate, 1001);
            var notFoundRequest_2 = await budgetController.GetUpcomingPaymentsSharedWithUserAsync(budgetDate, 1003);
            var notFoundRequest_3 = await budgetController.GetUpcomingPaymentsSharedWithUserAsync(budgetDate, 1004);

            var badRequest_1 = await budgetController.GetUpcomingPaymentsSharedWithUserAsync(budgetDate, -1001);
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

            var badRequest_2 = await budgetController.GetUpcomingPaymentsSharedWithUserAsync(new DateTime(), 1001);
            var badRequestActionResult_2 = badRequest_2 as BadRequestObjectResult;
            var badRequestResult_2 = badRequestActionResult_2?.Value as string;

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

            notFoundRequest_1.Should().BeOfType<NotFoundResult>();
            notFoundRequest_2.Should().BeOfType<NotFoundResult>();
            notFoundRequest_3.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_1?.Value.Should().NotBeNull();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_2?.Value.Should().NotBeNull();
            badRequestResult_2?.Contains("is not specified").Should().BeTrue();
        }

        [Fact]
        public async Task BudgetController_GetBudgetSummaryAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var budgetController = new BudgetController(_config, budgetApiService, _mapper);
            var budgetDate = DateTime.Parse("2024-01-01");

            //Act
            var okRequest_1 = await budgetController.GetBudgetSummaryAsync(budgetDate, 1);
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var budgetResult_1 = okActionResult_1?.Value as GetBudgetSummaryDto;

            var okRequest_2 = await budgetController.GetBudgetSummaryAsync(budgetDate, 2);
            var okActionResult_2 = okRequest_2 as OkObjectResult;
            var budgetResult_2 = okActionResult_2?.Value as GetBudgetSummaryDto;

            var okRequest_3 = await budgetController.GetBudgetSummaryAsync(budgetDate, 3);
            var okActionResult_3 = okRequest_3 as OkObjectResult;
            var budgetResult_3 = okActionResult_3?.Value as GetBudgetSummaryDto;

            var notFoundRequest_1 = await budgetController.GetBudgetSummaryAsync(budgetDate, 1001);

            var badRequest_1 = await budgetController.GetBudgetSummaryAsync(budgetDate, -1005);
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

            var badRequest_2 = await budgetController.GetBudgetSummaryAsync(new DateTime(), 1005);
            var badRequestActionResult_2 = badRequest_2 as BadRequestObjectResult;
            var badRequestResult_2 = badRequestActionResult_2?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            okActionResult_1.Should().NotBeNull();
            budgetResult_1.Should().NotBeNull();
            budgetResult_1?.Budget.Should().NotBeNull();
            budgetResult_1?.Budget.Status.Should().NotBeNullOrEmpty();
            budgetResult_1?.Budget.BudgetYear.Should()
                .NotBeBefore(DateTime.Parse("2024-01-01")).And
                .NotBeAfter(DateTime.Parse("2024-01-31"));
            budgetResult_1?.Budget.IsCompleted.Should().BeFalse();
            budgetResult_1?.Needs.Should().BeGreaterThanOrEqualTo(0);
            budgetResult_1?.Wants.Should().BeGreaterThanOrEqualTo(0);
            budgetResult_1?.Savings.Should().BeGreaterThanOrEqualTo(0);


            okRequest_2.Should().BeOfType<OkObjectResult>();
            okActionResult_2.Should().NotBeNull();
            budgetResult_2.Should().NotBeNull();
            budgetResult_2?.Budget.Should().NotBeNull();
            budgetResult_2?.Budget.Status.Should().NotBeNullOrEmpty();
            budgetResult_2?.Budget.BudgetYear.Should()
                .NotBeBefore(DateTime.Parse("2024-01-01")).And
                .NotBeAfter(DateTime.Parse("2024-01-31"));
            budgetResult_2?.Budget.IsCompleted.Should().BeFalse();
            budgetResult_2?.Needs.Should().BeGreaterThanOrEqualTo(0);
            budgetResult_2?.Wants.Should().BeGreaterThanOrEqualTo(0);
            budgetResult_2?.Savings.Should().BeGreaterThanOrEqualTo(0);

            okRequest_3.Should().BeOfType<OkObjectResult>();
            okActionResult_3.Should().NotBeNull();
            budgetResult_3.Should().NotBeNull();
            budgetResult_3?.Budget.Should().NotBeNull();
            budgetResult_3?.Budget.Status.Should().NotBeNullOrEmpty();
            budgetResult_3?.Budget.BudgetYear.Should()
                .NotBeBefore(DateTime.Parse("2024-01-01")).And
                .NotBeAfter(DateTime.Parse("2024-01-31"));
            budgetResult_3?.Budget.IsCompleted.Should().BeFalse();
            budgetResult_3?.Needs.Should().BeGreaterThanOrEqualTo(0);
            budgetResult_3?.Wants.Should().BeGreaterThanOrEqualTo(0);
            budgetResult_3?.Savings.Should().BeGreaterThanOrEqualTo(0);

            notFoundRequest_1.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_1?.Value.Should().NotBeNull();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_2?.Value.Should().NotBeNull();
            badRequestResult_2?.Contains("is not specified").Should().BeTrue();
        }

        [Fact]
        public async Task BudgetController_GetBudgetsAsync_ReturnsBudgetsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var budgetController = new BudgetController(_config, budgetApiService, _mapper);
            int budget_year = 2024;

            //Act
            var okRequest_1 = await budgetController.GetBudgetsAsync(budget_year, 1);
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var budgetsResult_1 = okActionResult_1?.Value as IEnumerable<GetBudgetSummaryDto>;

            var okRequest_2 = await budgetController.GetBudgetsAsync(budget_year, 2);
            var okActionResult_2 = okRequest_2 as OkObjectResult;
            var budgetsResult_2 = okActionResult_2?.Value as IEnumerable<GetBudgetSummaryDto>;

            var okRequest_3 = await budgetController.GetBudgetsAsync(budget_year, 3);
            var okActionResult_3 = okRequest_3 as OkObjectResult;
            var budgetsResult_3 = okActionResult_3?.Value as IEnumerable<GetBudgetSummaryDto>;

            var notFoundRequest_1 = await budgetController.GetBudgetsAsync(budget_year, 4);

            var badRequest_1 = await budgetController.GetBudgetsAsync(budget_year, -1005);
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

            var badRequest_2 = await budgetController.GetBudgetsAsync(-1, 1);
            var badRequestActionResult_2 = badRequest_2 as BadRequestObjectResult;
            var badRequestResult_2 = badRequestActionResult_2?.Value as string;

            var badRequest_3 = await budgetController.GetBudgetsAsync(99999, 1);
            var badRequestActionResult_3 = badRequest_3 as BadRequestObjectResult;
            var badRequestResult_3 = badRequestActionResult_3?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            okActionResult_1.Should().NotBeNull();
            budgetsResult_1.Should().NotBeNull();
            budgetsResult_1?.Count().Should().Be(12);
            budgetsResult_1?
                .Where(br => string.IsNullOrEmpty(br.Budget.Status))
                .ToList().Should().BeNullOrEmpty();
            budgetsResult_1?
                .Where(br => br.Budget.BudgetYear.Year != budget_year)
                .ToList().Should().BeNullOrEmpty();
            budgetsResult_1?
                .Where(br => br.Needs < 0 || br.Wants < 0 || br.Savings < 0)
                .ToList().Should().BeNullOrEmpty();

            okRequest_2.Should().BeOfType<OkObjectResult>();
            okActionResult_2.Should().NotBeNull();
            budgetsResult_2.Should().NotBeNull();
            budgetsResult_2?.Count().Should().Be(12);
            budgetsResult_2?
                .Where(br => string.IsNullOrEmpty(br.Budget.Status))
                .ToList().Should().BeNullOrEmpty();
            budgetsResult_2?
                .Where(br => br.Budget.BudgetYear.Year != budget_year)
                .ToList().Should().BeNullOrEmpty();
            budgetsResult_2?
                .Where(br => br.Needs < 0 || br.Wants < 0 || br.Savings < 0)
                .ToList().Should().BeNullOrEmpty();

            okRequest_3.Should().BeOfType<OkObjectResult>();
            okActionResult_3.Should().NotBeNull();
            budgetsResult_3.Should().NotBeNull();
            budgetsResult_3?.Count().Should().Be(12);
            budgetsResult_3?
                .Where(br => string.IsNullOrEmpty(br.Budget.Status))
                .ToList().Should().BeNullOrEmpty();
            budgetsResult_3?
                .Where(br => br.Budget.BudgetYear.Year != budget_year)
                .ToList().Should().BeNullOrEmpty();
            budgetsResult_3?
                .Where(br => br.Needs < 0 || br.Wants < 0 || br.Savings < 0)
                .ToList().Should().BeNullOrEmpty();

            notFoundRequest_1.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_1?.Value.Should().NotBeNull();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_2?.Value.Should().NotBeNull();
            badRequestResult_2?.Contains("is invalid").Should().BeTrue();

            badRequest_3.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_3?.Value.Should().NotBeNull();
            badRequestResult_3?.Contains("is invalid").Should().BeTrue();
        }

        [Fact]
        public async Task BudgetController_GetPaymentDetailsAsync_ReturnsPaymentOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var budgetController = new BudgetController(_config, budgetApiService, _mapper);
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
                    var okRequest = await budgetController.GetPaymentDetailsAsync(1, id);
                    var okActionResult = okRequest as OkObjectResult;
                    var paymentDetail = okActionResult?.Value as GetPrivatePaymentDto;
                    user1requests.Add(okActionResult);
                    paymentDetails.Add(paymentDetail);
                }

                if (id > 6 && id <= 11 || id == 13)
                {
                    var okRequest = await budgetController.GetPaymentDetailsAsync(2, id);
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
        public async Task BudgetController_GetPrivatePaymentsAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var budgetController = new BudgetController(_config, budgetApiService, _mapper);
            var budgetDate = DateTime.Parse("2024-01-01");

            //Act
            var okRequest_1 = await budgetController.GetPrivatePaymentsAsync(budgetDate, 1);
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var paymentsResult_1 = okActionResult_1?.Value as List<GetPrivatePaymentDto>;

            var okRequest_2 = await budgetController.GetPrivatePaymentsAsync(budgetDate, 2);
            var okActionResult_2 = okRequest_2 as OkObjectResult;
            var paymentsResult_2 = okActionResult_2?.Value as List<GetPrivatePaymentDto>;

            var notfoundRequest_1 = await budgetController.GetPrivatePaymentsAsync(budgetDate, 3);

            var badRequest_1 = await budgetController.GetPrivatePaymentsAsync(budgetDate, -1005);
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

            var badRequest_2 = await budgetController.GetPrivatePaymentsAsync(new DateTime(), 1);
            var badRequestActionResult_2 = badRequest_2 as BadRequestObjectResult;
            var badRequestResult_2 = badRequestActionResult_2?.Value as string;

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

            notfoundRequest_1.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_1?.Value.Should().NotBeNull();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_2?.Value.Should().NotBeNull();
            badRequestResult_2?.Contains("is not specified").Should().BeTrue();
        }

        [Fact]
        public async Task BudgetController_GetPaymentsSharedWithFriendAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var budgetController = new BudgetController(_config, budgetApiService, _mapper);
            var budgetDate = DateTime.Parse("2024-01-01");

            //Act
            var okRequest_1 = await budgetController.GetPaymentsSharedWithFriendAsync(budgetDate, 1);
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var paymentsResult_1 = okActionResult_1?.Value as IEnumerable<GetSharedPaymentToFriendDto>;

            var okRequest_2 = await budgetController.GetPaymentsSharedWithFriendAsync(budgetDate, 2);
            var okActionResult_2 = okRequest_2 as OkObjectResult;
            var paymentsResult_2 = okActionResult_2?.Value as IEnumerable<GetSharedPaymentToFriendDto>;

            var notFoundRequest_2 = await budgetController.GetPaymentsSharedWithFriendAsync(budgetDate, 3);
            var notFoundRequest_3 = await budgetController.GetPaymentsSharedWithFriendAsync(budgetDate, 4);

            var badRequest_1 = await budgetController.GetPaymentsSharedWithFriendAsync(budgetDate, -1001);
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

            var badRequest_2 = await budgetController.GetPaymentsSharedWithFriendAsync(new DateTime(), 1001);
            var badRequestActionResult_2 = badRequest_2 as BadRequestObjectResult;
            var badRequestResult_2 = badRequestActionResult_2?.Value as string;

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

            notFoundRequest_2.Should().BeOfType<NotFoundResult>();
            notFoundRequest_3.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_1?.Value.Should().NotBeNull();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_2?.Value.Should().NotBeNull();
            badRequestResult_2?.Contains("is not specified").Should().BeTrue();
        }
    }
}
