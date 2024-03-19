using AutoMapper;
using FluentAssertions;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDto;
using Pinionszek_API.Profiles;
using Pinionszek_API.Services.DatabaseServices.BudgetService;
using Pinionszek_API.Tests.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinionszek_API.Tests.Tests.UnitTests
{
    public class BudgetApiServiceTests
    {
        private readonly IMapper _mapper;
        public BudgetApiServiceTests()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PaymentProfile());
                cfg.AddProfile(new CategoryProfile());
            });
            _mapper = mockMapper.CreateMapper();
        }

        [Fact]
        public async Task BudgetApiService_GetBudgetDataAsync_ReturnsBudget()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);

            //Act
            var budget_of_idUser_1 = await budgetApiService.GetBudgetDataAsync(1, DateTime.Parse("2024-01-01"));
            var budget_of_idUser_2 = await budgetApiService.GetBudgetDataAsync(2, DateTime.Parse("2024-01-01"));
            var budget_of_idUser_3 = await budgetApiService.GetBudgetDataAsync(3, DateTime.Parse("2024-01-01"));
            var budget_of_idUser_4 = await budgetApiService.GetBudgetDataAsync(4, DateTime.Parse("2024-01-01"));
            var budget_of_invalid_user = await budgetApiService.GetBudgetDataAsync(1000, DateTime.Parse("2024-01-01"));

            //Assert
            budget_of_idUser_1.Should().NotBeNull();
            budget_of_idUser_1?.BudgetStatus.Name.Should().Be("OPEND");
            budget_of_idUser_1?.IsCompleted.Should().BeFalse();
            budget_of_idUser_1?.OpendDate.Should().Be(DateTime.Parse("2024-01-01"));

            budget_of_idUser_2.Should().NotBeNull();
            budget_of_idUser_2?.BudgetStatus.Name.Should().Be("OPEND");
            budget_of_idUser_2?.IsCompleted.Should().BeFalse();
            budget_of_idUser_2?.OpendDate.Should().Be(DateTime.Parse("2023-12-29"));

            budget_of_idUser_3.Should().NotBeNull();
            budget_of_idUser_3?.BudgetStatus.Name.Should().Be("OPEND");
            budget_of_idUser_3?.IsCompleted.Should().BeFalse();
            budget_of_idUser_3?.OpendDate.Should().Be(DateTime.Parse("2024-01-11"));

            budget_of_idUser_4.Should().BeNull();
            budget_of_invalid_user.Should().BeNull();
        }

        [Fact]
        public async Task BudgetApiService_GetPaymentsAsync_ReturnsPayments()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);

            //Act
            var payments_of_idUser_1 = await budgetApiService.GetPaymentsAsync(1);
            var payments_of_idUser_2 = await budgetApiService.GetPaymentsAsync(13);
            var payments_of_non_existing_budget = await budgetApiService.GetPaymentsAsync(37);

            //Assert
            payments_of_idUser_1.Should().NotBeEmpty();
            payments_of_idUser_1.Count().Should().Be(7);
            payments_of_idUser_1
                .Where(p => p.DetailedCategory == null ||
                    string.IsNullOrEmpty(p.DetailedCategory.Name) ||
                    p.DetailedCategory.IdGeneralCategory == 0 ||
                    p.DetailedCategory.GeneralCategory.IdGeneralCategory == 0 ||
                    string.IsNullOrEmpty(p.DetailedCategory?.GeneralCategory.Name))
                .ToList().Count()
                .Should().Be(0);
            payments_of_idUser_1
                .Where(p => p.IdPaymentStatus == 0)
                .ToList().Count()
                .Should().Be(0);
            payments_of_idUser_1
                .Where(p => string.IsNullOrEmpty(p.Name))
                .ToList().Count().Should().Be(0);

            payments_of_idUser_2.Should().NotBeEmpty();
            payments_of_idUser_2.Count().Should().Be(6);
            payments_of_idUser_2
                .Where(p => p.DetailedCategory == null ||
                    string.IsNullOrEmpty(p.DetailedCategory.Name) ||
                    p.DetailedCategory.IdGeneralCategory == 0 ||
                    p.DetailedCategory.GeneralCategory.IdGeneralCategory == 0 ||
                    string.IsNullOrEmpty(p.DetailedCategory?.GeneralCategory.Name))
                .ToList().Count()
                .Should().Be(0);
            payments_of_idUser_2
                .Where(p => p.IdPaymentStatus == 0)
                .ToList().Count()
                .Should().Be(0);
            payments_of_idUser_2
                .Where(p => string.IsNullOrEmpty(p.Name))
                .ToList().Count().Should().Be(0);


            payments_of_non_existing_budget.Should().BeEmpty();
        }

        [Fact]
        public async Task BudgetApiService_GetSharedPaymentDataAsync_ReturnsSharedPaymentOrNull()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);

            //Act
            var shared_idPayment_1 = await budgetApiService.GetSharedPaymentDataAsync(1);
            var shared_idPayment_4 = await budgetApiService.GetSharedPaymentDataAsync(4);
            var shared_idPayment_10 = await budgetApiService.GetSharedPaymentDataAsync(10);
            var shared_non_existing_payment = await budgetApiService.GetSharedPaymentDataAsync(100);

            //Assert
            shared_idPayment_1.Should().NotBeNull();
            shared_idPayment_1?.IdPayment.Should().Be(1);
            shared_idPayment_1?.IdFriend.Should().Be(1);

            shared_idPayment_4.Should().NotBeNull();
            shared_idPayment_4?.IdPayment.Should().Be(4);
            shared_idPayment_4?.IdFriend.Should().Be(1);

            shared_idPayment_10.Should().NotBeNull();
            shared_idPayment_10?.IdPayment.Should().Be(10);
            shared_idPayment_10?.IdFriend.Should().Be(3);

            shared_non_existing_payment.Should().BeNull();
        }

        [Fact]
        public async Task BudgetApiService_GetFriendNameAndTagAsync_ReturnsString()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);

            //Act
            var result_of_idSharedPayment_1 = await budgetApiService.GetFriendReceiveNameAndTagAsync(1);
            var result_of_idSharedPayment_2 = await budgetApiService.GetFriendReceiveNameAndTagAsync(2);
            var result_of_idSharedPayment_3 = await budgetApiService.GetFriendReceiveNameAndTagAsync(3);
            var result_empty = await budgetApiService.GetFriendReceiveNameAndTagAsync(1001);

            //Assert
            result_of_idSharedPayment_1.Should().NotBeNull();
            result_of_idSharedPayment_1.Should().BeOfType<(string?, int?)>();
            result_of_idSharedPayment_1.Item1.Should().Be("test2");
            result_of_idSharedPayment_1.Item2.Should().Be(1002);

            result_of_idSharedPayment_2.Should().NotBeNull();
            result_of_idSharedPayment_2.Should().BeOfType<(string?, int?)>();
            result_of_idSharedPayment_2.Item1.Should().Be("test2");
            result_of_idSharedPayment_2.Item2.Should().Be(1002);

            result_of_idSharedPayment_3.Should().NotBeNull();
            result_of_idSharedPayment_3.Should().BeOfType<(string?, int?)>();
            result_of_idSharedPayment_3.Item1.Should().Be("test1");
            result_of_idSharedPayment_3.Item2.Should().Be(1001);

            result_empty.Item1.Should().BeNull();
            result_empty.Item2.Should().BeNull();
        }

        [Fact]
        public async Task BudgetApiService_GetAssignedPayments_ReturnsPaymentsOrNotfoundOrBadRequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);

            //Act
            var assignedPaymentsFriendTag_1001 = await budgetApiService.GetAssignedPaymentsAsync(1001);
            var assignedPaymentsFriendTag_1002 = await budgetApiService.GetAssignedPaymentsAsync(1002);
            var assignedPaymentsFriendTag_1003 = await budgetApiService.GetAssignedPaymentsAsync(1003);
            var assignedPaymentsFriendTag_1004 = await budgetApiService.GetAssignedPaymentsAsync(1004);

            //Assert
            assignedPaymentsFriendTag_1001.Should().NotBeNullOrEmpty();
            assignedPaymentsFriendTag_1001.Should().BeOfType<List<Payment>>();
            assignedPaymentsFriendTag_1001.Count().Should().Be(1);
            assignedPaymentsFriendTag_1001
                .Where(ap => ap.SharedPayment == null)
                .ToList().Count().Should().Be(0);
            assignedPaymentsFriendTag_1001
                .Where(ap => ap.DetailedCategory == null ||
                    string.IsNullOrEmpty(ap.DetailedCategory.Name) ||
                    ap.DetailedCategory.IdGeneralCategory == 0 ||
                    ap.DetailedCategory.GeneralCategory.IdGeneralCategory == 0 ||
                    string.IsNullOrEmpty(ap.DetailedCategory?.GeneralCategory.Name))
                .ToList().Count().Should().Be(0);
            assignedPaymentsFriendTag_1001
                .Where(ap => ap.IdPaymentStatus == 0)
                .ToList().Count()
                .Should().Be(0);
            assignedPaymentsFriendTag_1001
                .Where(ap => string.IsNullOrEmpty(ap.Name))
                .ToList().Count().Should().Be(0);

            assignedPaymentsFriendTag_1002.Should().NotBeNullOrEmpty();
            assignedPaymentsFriendTag_1002.Should().BeOfType<List<Payment>>();
            assignedPaymentsFriendTag_1002.Count().Should().Be(2);
            assignedPaymentsFriendTag_1002
                .Where(ap => ap.SharedPayment == null)
                .ToList().Count().Should().Be(0);
            assignedPaymentsFriendTag_1002
                .Where(ap => ap.DetailedCategory == null ||
                    string.IsNullOrEmpty(ap.DetailedCategory.Name) ||
                    ap.DetailedCategory.IdGeneralCategory == 0 ||
                    ap.DetailedCategory.GeneralCategory.IdGeneralCategory == 0 ||
                    string.IsNullOrEmpty(ap.DetailedCategory?.GeneralCategory.Name))
                .ToList().Count().Should().Be(0);
            assignedPaymentsFriendTag_1002
                .Where(ap => ap.IdPaymentStatus == 0)
                .ToList().Count()
                .Should().Be(0);
            assignedPaymentsFriendTag_1002
                .Where(ap => string.IsNullOrEmpty(ap.Name))
                .ToList().Count().Should().Be(0);

            assignedPaymentsFriendTag_1003.Should().BeNullOrEmpty();
            assignedPaymentsFriendTag_1004.Should().BeNullOrEmpty();
        }
    }
}
