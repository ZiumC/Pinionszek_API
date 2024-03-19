using AutoMapper;
using Castle.Components.DictionaryAdapter.Xml;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Pinionszek_API.Controllers;
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
        public BudgetControllerTests()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PaymentProfile());
                cfg.AddProfile(new CategoryProfile());
            });
            _mapper = mockMapper.CreateMapper();
        }

        [Fact]
        public async Task BudgetController_GetUpcomingPrivatePaymentsAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var budgetController = new BudgetController(budgetApiService, _mapper);

            //Act
            var okRequest_1 = await budgetController.GetUpcomingPrivatePaymentsAsync(1, DateTime.Parse("2024-01-01"));
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var paymentsResult_1 = okActionResult_1?.Value as IEnumerable<GetPrivatePaymentDto>;

            var okRequest_2 = await budgetController.GetUpcomingPrivatePaymentsAsync(2, DateTime.Parse("2024-01-01"));
            var okActionResult_2 = okRequest_2 as OkObjectResult;
            var paymentsResult_2 = okActionResult_2?.Value as IEnumerable<GetPrivatePaymentDto>;

            var notFoundRequest_1 = await budgetController.GetUpcomingPrivatePaymentsAsync(3, DateTime.Parse("2024-01-01"));
            var notFoundRequest_2 = await budgetController.GetUpcomingPrivatePaymentsAsync(4, DateTime.Parse("2024-01-01"));

            var badRequest_1 = await budgetController.GetUpcomingPrivatePaymentsAsync(-10, DateTime.Parse("2024-01-01"));
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

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
            badRequestResult_1?.Contains("User ID is invalid").Should().BeTrue();
        }


        [Fact]
        public async Task BudgetController_GetUpcomingPaymentsSharedWithFriendAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var budgetController = new BudgetController(budgetApiService, _mapper);

            //Act
            var okRequest_1 = await budgetController.GetUpcomingPaymentsSharedWithFriendAsync(1, DateTime.Parse("2024-01-01"));
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var paymentsResult_1 = okActionResult_1?.Value as IEnumerable<GetSharedPaymentToFriendDto>;

            var notFoundRequest_1 = await budgetController.GetUpcomingPaymentsSharedWithFriendAsync(2, DateTime.Parse("2024-01-01"));
            var notFoundRequest_2 = await budgetController.GetUpcomingPaymentsSharedWithFriendAsync(3, DateTime.Parse("2024-01-01"));
            var notFoundRequest_3 = await budgetController.GetUpcomingPaymentsSharedWithFriendAsync(4, DateTime.Parse("2024-01-01"));

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
        }

        [Fact]
        public async Task BudgetController_GetUpcomingPaymentsSharedWithUserAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var budgetController = new BudgetController(budgetApiService, _mapper);

            //Act
            var okRequest_1 = await budgetController.GetUpcomingPaymentsSharedWithUserAsync(1002, DateTime.Parse("2024-01-01"));
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var paymentsResult_1 = okActionResult_1?.Value as IEnumerable<GetAssignedPaymentToUserDto>;

            var notFoundRequest_1 = await budgetController.GetUpcomingPaymentsSharedWithFriendAsync(1001, DateTime.Parse("2024-01-01"));
            var notFoundRequest_2 = await budgetController.GetUpcomingPaymentsSharedWithFriendAsync(1003, DateTime.Parse("2024-01-01"));
            var notFoundRequest_3 = await budgetController.GetUpcomingPaymentsSharedWithFriendAsync(1004, DateTime.Parse("2024-01-01"));

            var badRequest_1 = await budgetController.GetUpcomingPrivatePaymentsAsync(-1005, DateTime.Parse("2024-01-01"));
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

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
            badRequestResult_1?.Contains("User ID is invalid").Should().BeTrue();
        }

        [Fact]
        public async Task BudgetController_GetBudgetSummaryAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var budgetController = new BudgetController(budgetApiService, _mapper);

            //Act
            var okRequest_1 = await budgetController.GetBudgetSummaryAsync(1, DateTime.Parse("2024-01-01"));
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var budgetResult_1 = okActionResult_1?.Value as GetBudgetSummaryDto;

            var okRequest_2 = await budgetController.GetBudgetSummaryAsync(2, DateTime.Parse("2024-01-01"));
            var okActionResult_2 = okRequest_2 as OkObjectResult;
            var budgetResult_2 = okActionResult_2?.Value as GetBudgetSummaryDto;

            var okRequest_3 = await budgetController.GetBudgetSummaryAsync(3, DateTime.Parse("2024-01-01"));
            var okActionResult_3 = okRequest_3 as OkObjectResult;
            var budgetResult_3 = okActionResult_3?.Value as GetBudgetSummaryDto;

            var notFoundRequest_1 = await budgetController.GetUpcomingPaymentsSharedWithFriendAsync(1001, DateTime.Parse("2024-01-01"));

            var badRequest_1 = await budgetController.GetUpcomingPrivatePaymentsAsync(-1005, DateTime.Parse("2024-01-01"));
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            okActionResult_1.Should().NotBeNull();
            budgetResult_1.Should().NotBeNull();
            budgetResult_1?.Budget.Should().NotBeNull();
            budgetResult_1?.Budget.Status.Should().NotBeNullOrEmpty();
            budgetResult_1?.Budget.BudgetYear.Should().NotBeBefore(DateTime.Parse("2024-01-01"));
            budgetResult_1?.Budget.BudgetYear.Should().NotBeAfter(DateTime.Parse("2024-01-31"));
            budgetResult_1?.Budget.IsCompleted.Should().BeFalse();
            budgetResult_1?.Needs.Should().BePositive();
            budgetResult_1?.Wants.Should().BePositive();
            budgetResult_1?.Savings.Should().BePositive();
            budgetResult_1?.Unused.Should().BePositive();


            okRequest_2.Should().BeOfType<OkObjectResult>();
            okActionResult_2.Should().NotBeNull();
            budgetResult_2.Should().NotBeNull();
            budgetResult_2?.Budget.Should().NotBeNull();
            budgetResult_2?.Budget.Status.Should().NotBeNullOrEmpty();
            budgetResult_2?.Budget.BudgetYear.Should().NotBeBefore(DateTime.Parse("2024-01-01"));
            budgetResult_2?.Budget.BudgetYear.Should().NotBeAfter(DateTime.Parse("2024-01-31"));
            budgetResult_2?.Budget.IsCompleted.Should().BeFalse();
            budgetResult_2?.Needs.Should().BePositive();
            budgetResult_2?.Wants.Should().BePositive();
            budgetResult_2?.Savings.Should().BePositive();
            budgetResult_2?.Unused.Should().BePositive();

            okRequest_3.Should().BeOfType<OkObjectResult>();
            okActionResult_3.Should().NotBeNull();
            budgetResult_3.Should().NotBeNull();
            budgetResult_3?.Budget.Should().NotBeNull();
            budgetResult_3?.Budget.Status.Should().NotBeNullOrEmpty();
            budgetResult_3?.Budget.BudgetYear.Should().NotBeBefore(DateTime.Parse("2024-01-01"));
            budgetResult_3?.Budget.BudgetYear.Should().NotBeAfter(DateTime.Parse("2024-01-31"));
            budgetResult_3?.Budget.IsCompleted.Should().BeFalse();
            budgetResult_3?.Needs.Should().BePositive();
            budgetResult_3?.Wants.Should().BePositive();
            budgetResult_3?.Savings.Should().BePositive();
            budgetResult_3?.Unused.Should().BePositive();

            notFoundRequest_1.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_1?.Value.Should().NotBeNull();
            badRequestResult_1?.Contains("User ID is invalid").Should().BeTrue();
        }
    }
}
