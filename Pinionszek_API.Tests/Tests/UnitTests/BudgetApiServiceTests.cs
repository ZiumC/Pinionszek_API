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

        [Fact]
        public async Task BudgetApiService_GetBudgetDataAsync_ReturnsBudgetOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);

            //Act
            var budgetOfIdUser_1 = await budgetApiService.GetBudgetDataAsync(1, DateTime.Parse("2024-01-01"));
            var budgetOfIdUser_2 = await budgetApiService.GetBudgetDataAsync(2, DateTime.Parse("2024-01-01"));
            var budgetOfIdUser_3 = await budgetApiService.GetBudgetDataAsync(3, DateTime.Parse("2024-01-01"));
            var budgetOfIdUser_4 = await budgetApiService.GetBudgetDataAsync(4, DateTime.Parse("2024-01-01"));
            var budgetOfInvalidUser = await budgetApiService.GetBudgetDataAsync(1000, DateTime.Parse("2024-01-01"));

            //Assert
            budgetOfIdUser_1.Should().NotBeNull();
            budgetOfIdUser_1?.BudgetStatus.Name.Should().Be("OPEND");
            budgetOfIdUser_1?.IsCompleted.Should().BeFalse();
            budgetOfIdUser_1?.OpendDate.Should().Be(DateTime.Parse("2024-01-01"));

            budgetOfIdUser_2.Should().NotBeNull();
            budgetOfIdUser_2?.BudgetStatus.Name.Should().Be("OPEND");
            budgetOfIdUser_2?.IsCompleted.Should().BeFalse();
            budgetOfIdUser_2?.OpendDate.Should().Be(DateTime.Parse("2023-12-29"));

            budgetOfIdUser_3.Should().NotBeNull();
            budgetOfIdUser_3?.BudgetStatus.Name.Should().Be("OPEND");
            budgetOfIdUser_3?.IsCompleted.Should().BeFalse();
            budgetOfIdUser_3?.OpendDate.Should().Be(DateTime.Parse("2024-01-11"));

            budgetOfIdUser_4.Should().BeNull();
            budgetOfInvalidUser.Should().BeNull();
        }

        [Fact]
        public async Task BudgetApiService_GetPaymentsAsync_ReturnsPaymentsOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);

            //Act
            var paymentsOfIdUser_1 = await budgetApiService.GetPaymentsAsync(1);
            var paymentsOfIdUser_2 = await budgetApiService.GetPaymentsAsync(13);
            var paymentsOfNonExistingBudget = await budgetApiService.GetPaymentsAsync(37);

            //Assert
            paymentsOfIdUser_1.Should().NotBeEmpty();
            paymentsOfIdUser_1.Count().Should().Be(7);
            paymentsOfIdUser_1
                .Where(p => p.DetailedCategory == null ||
                    string.IsNullOrEmpty(p.DetailedCategory.Name) ||
                    p.DetailedCategory.IdGeneralCategory == 0 ||
                    p.DetailedCategory.GeneralCategory.IdGeneralCategory == 0 ||
                    string.IsNullOrEmpty(p.DetailedCategory?.GeneralCategory.Name))
                .ToList().Count()
                .Should().Be(0);
            paymentsOfIdUser_1
                .Where(p => p.IdPaymentStatus == 0)
                .ToList().Count()
                .Should().Be(0);
            paymentsOfIdUser_1
                .Where(p => string.IsNullOrEmpty(p.Name))
                .ToList().Count().Should().Be(0);

            paymentsOfIdUser_2.Should().NotBeEmpty();
            paymentsOfIdUser_2.Count().Should().Be(6);
            paymentsOfIdUser_2
                .Where(p => p.DetailedCategory == null ||
                    string.IsNullOrEmpty(p.DetailedCategory.Name) ||
                    p.DetailedCategory.IdGeneralCategory == 0 ||
                    p.DetailedCategory.GeneralCategory.IdGeneralCategory == 0 ||
                    string.IsNullOrEmpty(p.DetailedCategory?.GeneralCategory.Name))
                .ToList().Count()
                .Should().Be(0);
            paymentsOfIdUser_2
                .Where(p => p.IdPaymentStatus == 0)
                .ToList().Count()
                .Should().Be(0);
            paymentsOfIdUser_2
                .Where(p => string.IsNullOrEmpty(p.Name))
                .ToList().Count().Should().Be(0);


            paymentsOfNonExistingBudget.Should().BeEmpty();
        }

        [Fact]
        public async Task BudgetApiService_GetSharedPaymentDataAsync_ReturnsSharedPaymentOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);

            //Act
            var sharedIdPayment_1 = await budgetApiService.GetSharedPaymentDataAsync(1);
            var sharedIdPayment_4 = await budgetApiService.GetSharedPaymentDataAsync(4);
            var sharedIdPayment_10 = await budgetApiService.GetSharedPaymentDataAsync(10);
            var sharedNonExistingPayment = await budgetApiService.GetSharedPaymentDataAsync(100);

            //Assert
            sharedIdPayment_1.Should().NotBeNull();
            sharedIdPayment_1?.IdPayment.Should().Be(1);
            sharedIdPayment_1?.IdFriend.Should().Be(1);

            sharedIdPayment_4.Should().NotBeNull();
            sharedIdPayment_4?.IdPayment.Should().Be(4);
            sharedIdPayment_4?.IdFriend.Should().Be(1);

            sharedIdPayment_10.Should().NotBeNull();
            sharedIdPayment_10?.IdPayment.Should().Be(10);
            sharedIdPayment_10?.IdFriend.Should().Be(3);

            sharedNonExistingPayment.Should().BeNull();
        }

        [Fact]
        public async Task BudgetApiService_GetFriendReceiveNameAndTagAsync_ReturnsStringOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);

            //Act
            var resultOfIdSharedPayment_1 = await budgetApiService.GetFriendReceiveNameAndTagAsync(1);
            var resultOfIdSharedPayment_2 = await budgetApiService.GetFriendReceiveNameAndTagAsync(2);
            var resultOfIdSharedPayment_3 = await budgetApiService.GetFriendReceiveNameAndTagAsync(3);
            var emptyResult = await budgetApiService.GetFriendReceiveNameAndTagAsync(1001);

            //Assert
            resultOfIdSharedPayment_1.Should().NotBeNull();
            resultOfIdSharedPayment_1.Should().BeOfType<(string?, int?)>();
            resultOfIdSharedPayment_1.Item1.Should().Be("test2");
            resultOfIdSharedPayment_1.Item2.Should().Be(1002);

            resultOfIdSharedPayment_2.Should().NotBeNull();
            resultOfIdSharedPayment_2.Should().BeOfType<(string?, int?)>();
            resultOfIdSharedPayment_2.Item1.Should().Be("test2");
            resultOfIdSharedPayment_2.Item2.Should().Be(1002);

            resultOfIdSharedPayment_3.Should().NotBeNull();
            resultOfIdSharedPayment_3.Should().BeOfType<(string?, int?)>();
            resultOfIdSharedPayment_3.Item1.Should().Be("test1");
            resultOfIdSharedPayment_3.Item2.Should().Be(1001);

            emptyResult.Item1.Should().BeNull();
            emptyResult.Item2.Should().BeNull();
        }

        [Fact]
        public async Task BudgetApiService_GetFriendSenderNameAndTagAsync_ReturnsStringOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);

            //Act
            var resultOfIdSharedPayment_1 = await budgetApiService.GetFriendSenderNameAndTagAsync(1);
            var resultOfIdSharedPayment_2 = await budgetApiService.GetFriendSenderNameAndTagAsync(2);
            var resultOfIdSharedPayment_3 = await budgetApiService.GetFriendSenderNameAndTagAsync(3);
            var result_empty = await budgetApiService.GetFriendSenderNameAndTagAsync(1001);

            //Assert
            resultOfIdSharedPayment_1.Should().NotBeNull();
            resultOfIdSharedPayment_1.Should().BeOfType<(string?, int?)>();
            resultOfIdSharedPayment_1.Item1.Should().Be("test1");
            resultOfIdSharedPayment_1.Item2.Should().Be(1001);

            resultOfIdSharedPayment_2.Should().NotBeNull();
            resultOfIdSharedPayment_2.Should().BeOfType<(string?, int?)>();
            resultOfIdSharedPayment_2.Item1.Should().Be("test1");
            resultOfIdSharedPayment_2.Item2.Should().Be(1001);

            resultOfIdSharedPayment_3.Should().NotBeNull();
            resultOfIdSharedPayment_3.Should().BeOfType<(string?, int?)>();
            resultOfIdSharedPayment_3.Item1.Should().Be("test2");
            resultOfIdSharedPayment_3.Item2.Should().Be(1002);

            result_empty.Item1.Should().BeNull();
            result_empty.Item2.Should().BeNull();
        }

        [Fact]
        public async Task BudgetApiService_GetAssignedPaymentsAsync_ReturnsPaymentsOrNotfound()
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

        [Fact]
        public async Task BudgetApiService_GetUserSettingsAsync_ReturnsPaymentsOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);

            //Act
            var userSettingsOfIdUser_1 = await budgetApiService.GetUserSettingsAsync(1);
            var userSettingsOfIdUser_2 = await budgetApiService.GetUserSettingsAsync(2);
            var userSettingsOfIdUser_3 = await budgetApiService.GetUserSettingsAsync(3);
            var userSettingsOfIdUser_4 = await budgetApiService.GetUserSettingsAsync(4);
            var userSettingsOfNonExistingUser = await budgetApiService.GetUserSettingsAsync(10001);

            //Assert
            userSettingsOfIdUser_1.Should().NotBeNull();
            userSettingsOfIdUser_2.Should().NotBeNull();
            userSettingsOfIdUser_3.Should().NotBeNull();
            userSettingsOfIdUser_4.Should().NotBeNull();
            userSettingsOfNonExistingUser.Should().BeNull();

        }

        [Fact]
        public async Task BudgetApiService_GetBudgetsAsync_ReturnsBudgetsOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);

            //Act
            var budgetOfIdUser_1 = await budgetApiService.GetBudgetsAsync(1);
            var budgetOfIdUser_2 = await budgetApiService.GetBudgetsAsync(2);
            var budgetOfIdUser_3 = await budgetApiService.GetBudgetsAsync(3);
            var budgetOfIdUser_4 = await budgetApiService.GetBudgetsAsync(4);
            var budgetOfNonExistingUser = await budgetApiService.GetBudgetsAsync(10001);

            //Assert
            budgetOfIdUser_1.Should().NotBeNull();
            budgetOfIdUser_1.Count().Should().Be(12);
            budgetOfIdUser_1
                .Where(b => b.BudgetStatus == null ||
                    string.IsNullOrEmpty(b.BudgetStatus.Name))
                .Should().BeNullOrEmpty();
            budgetOfIdUser_1
                .Where(b => b.Payments != null)
                .Should().BeNullOrEmpty();


            budgetOfIdUser_2.Should().NotBeNull();
            budgetOfIdUser_2.Count().Should().Be(12);
            budgetOfIdUser_2
                .Where(b => b.BudgetStatus == null ||
                    string.IsNullOrEmpty(b.BudgetStatus.Name))
                .Should().BeNullOrEmpty();
            budgetOfIdUser_2
                .Where(b => b.Payments != null)
                .Should().BeNullOrEmpty();

            budgetOfIdUser_3.Should().NotBeNull();
            budgetOfIdUser_3.Count().Should().Be(12);
            budgetOfIdUser_3
                .Where(b => b.BudgetStatus == null ||
                    string.IsNullOrEmpty(b.BudgetStatus.Name))
                .Should().BeNullOrEmpty();
            budgetOfIdUser_3
                .Where(b => b.Payments != null)
                .Should().BeNullOrEmpty();


            budgetOfIdUser_4.Should().BeNullOrEmpty();
            budgetOfNonExistingUser.Should().BeNullOrEmpty();
        }
    }
}
