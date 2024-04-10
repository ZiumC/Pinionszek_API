﻿using AutoMapper;
using FluentAssertions;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDto;
using Pinionszek_API.Profiles;
using Pinionszek_API.Services.DatabaseServices.BudgetService;
using Pinionszek_API.Services.DatabaseServices.UserService;
using Pinionszek_API.Tests.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinionszek_API.Tests.Tests.UnitTests.ApiServices
{
    public class BudgetApiServiceTests
    {
        private readonly DateTime _budgetDate = DateTime.Parse("2024-01-01");
        private readonly int _user_1 = 1;
        private readonly int _user_2 = 2;
        private readonly int _user_3 = 3;
        private readonly int _user_4 = 4;
        private readonly int _user_5 = 100;

        [Fact]
        public async Task BudgetApiService_GetBudgetDataAsync_ReturnsBudgetOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);

            //Act
            var budget_1 = await budgetApiService.GetBudgetDataAsync(_user_1, _budgetDate);
            var budget_2 = await budgetApiService.GetBudgetDataAsync(_user_2, _budgetDate);
            var budget_3 = await budgetApiService.GetBudgetDataAsync(_user_3, _budgetDate);
            var budget_4 = await budgetApiService.GetBudgetDataAsync(_user_4, _budgetDate);
            var budget_5 = await budgetApiService.GetBudgetDataAsync(_user_5, _budgetDate);

            //Assert
            budget_1.Should().NotBeNull();
            budget_1?.BudgetStatus.Name.Should().Be("OPEND");
            budget_1?.IsCompleted.Should().BeFalse();
            budget_1?.OpendDate.Should().Be(_budgetDate);

            budget_2.Should().NotBeNull();
            budget_2?.BudgetStatus.Name.Should().Be("OPEND");
            budget_2?.IsCompleted.Should().BeFalse();
            budget_2?.OpendDate.Should().Be(_budgetDate);

            budget_3.Should().NotBeNull();
            budget_3?.BudgetStatus.Name.Should().Be("OPEND");
            budget_3?.IsCompleted.Should().BeFalse();
            budget_3?.OpendDate.Should().Be(_budgetDate);

            budget_4.Should().BeNull();
            budget_5.Should().BeNull();
        }

        [Fact]
        public async Task BudgetApiService_GetFriendReceiveNameAndTagAsync_ReturnsStringOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);

            //Act
            var sharedPaymentResult_1 = await budgetApiService.GetFriendReceiveNameAndTagAsync(_user_1);
            var sharedPaymentResult_2 = await budgetApiService.GetFriendReceiveNameAndTagAsync(_user_2);
            var sharedPaymentResult_3 = await budgetApiService.GetFriendReceiveNameAndTagAsync(_user_3);
            var sharedPaymentResult_4 = await budgetApiService.GetFriendReceiveNameAndTagAsync(_user_5);

            //Assert
            sharedPaymentResult_1.Should().NotBeNull();
            sharedPaymentResult_1.Should().BeOfType<(string?, int?)>();
            sharedPaymentResult_1.Item1.Should().Be("test2");
            sharedPaymentResult_1.Item2.Should().Be(1002);

            sharedPaymentResult_2.Should().NotBeNull();
            sharedPaymentResult_2.Should().BeOfType<(string?, int?)>();
            sharedPaymentResult_2.Item1.Should().Be("test2");
            sharedPaymentResult_2.Item2.Should().Be(1002);

            sharedPaymentResult_3.Should().NotBeNull();
            sharedPaymentResult_3.Should().BeOfType<(string?, int?)>();
            sharedPaymentResult_3.Item1.Should().Be("test1");
            sharedPaymentResult_3.Item2.Should().Be(1001);

            sharedPaymentResult_4.Item1.Should().BeNull();
            sharedPaymentResult_4.Item2.Should().BeNull();
        }

        [Fact]
        public async Task BudgetApiService_GetFriendSenderNameAndTagAsync_ReturnsStringOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);

            //Act
            var sharedPaymentResult_1 = await budgetApiService.GetFriendSenderNameAndTagAsync(_user_1);
            var sharedPaymentResult_2 = await budgetApiService.GetFriendSenderNameAndTagAsync(_user_2);
            var sharedPaymentResult_3 = await budgetApiService.GetFriendSenderNameAndTagAsync(_user_3);
            var sharedPaymentResult_4 = await budgetApiService.GetFriendSenderNameAndTagAsync(_user_5);

            //Assert
            sharedPaymentResult_1.Should().NotBeNull();
            sharedPaymentResult_1.Should().BeOfType<(string?, int?)>();
            sharedPaymentResult_1.Item1.Should().Be("test1");
            sharedPaymentResult_1.Item2.Should().Be(1001);

            sharedPaymentResult_2.Should().NotBeNull();
            sharedPaymentResult_2.Should().BeOfType<(string?, int?)>();
            sharedPaymentResult_2.Item1.Should().Be("test1");
            sharedPaymentResult_2.Item2.Should().Be(1001);

            sharedPaymentResult_3.Should().NotBeNull();
            sharedPaymentResult_3.Should().BeOfType<(string?, int?)>();
            sharedPaymentResult_3.Item1.Should().Be("test2");
            sharedPaymentResult_3.Item2.Should().Be(1002);

            sharedPaymentResult_4.Item1.Should().BeNull();
            sharedPaymentResult_4.Item2.Should().BeNull();
        }

        [Fact]
        public async Task BudgetApiService_GetUserSettingsAsync_ReturnsPaymentsOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);

            //Act
            var userSettings_1 = await budgetApiService.GetUserSettingsAsync(_user_1);
            var userSettings_2 = await budgetApiService.GetUserSettingsAsync(_user_2);
            var userSettings_3 = await budgetApiService.GetUserSettingsAsync(_user_3);
            var userSettings_4 = await budgetApiService.GetUserSettingsAsync(_user_4);
            var userSettings_5 = await budgetApiService.GetUserSettingsAsync(_user_5);

            //Assert
            userSettings_1.Should().NotBeNull();
            userSettings_2.Should().NotBeNull();
            userSettings_3.Should().NotBeNull();
            userSettings_4.Should().NotBeNull();
            userSettings_5.Should().BeNull();

        }

        [Fact]
        public async Task BudgetApiService_GetBudgetsAsync_ReturnsBudgetsOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);

            //Act
            var budget_1 = await budgetApiService.GetBudgetsAsync(_user_1);
            var budget_2 = await budgetApiService.GetBudgetsAsync(_user_2);
            var budget_3 = await budgetApiService.GetBudgetsAsync(_user_3);
            var budget_4 = await budgetApiService.GetBudgetsAsync(_user_4);
            var budget_5 = await budgetApiService.GetBudgetsAsync(_user_5);

            //Assert
            budget_1.Should().NotBeNull();
            budget_1.Count().Should().Be(12);
            budget_1
                .Where(b => b.BudgetStatus == null ||
                    string.IsNullOrEmpty(b.BudgetStatus.Name))
                .Should().BeNullOrEmpty();
            budget_1
                .Where(b => b.Payments != null)
                .Should().BeNullOrEmpty();


            budget_2.Should().NotBeNull();
            budget_2.Count().Should().Be(12);
            budget_2
                .Where(b => b.BudgetStatus == null ||
                    string.IsNullOrEmpty(b.BudgetStatus.Name))
                .Should().BeNullOrEmpty();
            budget_2
                .Where(b => b.Payments != null)
                .Should().BeNullOrEmpty();

            budget_3.Should().NotBeNull();
            budget_3.Count().Should().Be(12);
            budget_3
                .Where(b => b.BudgetStatus == null ||
                    string.IsNullOrEmpty(b.BudgetStatus.Name))
                .Should().BeNullOrEmpty();
            budget_3
                .Where(b => b.Payments != null)
                .Should().BeNullOrEmpty();


            budget_4.Should().BeNullOrEmpty();
            budget_5.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task BudgetApiService_GetDefaultGeneralCategoriesAsync_ReturnsCategoriesOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);

            //Act
            var defaultCategories = await budgetApiService.GetDefaultGeneralCategoriesAsync();

            //Assert
            defaultCategories.Should().NotBeNullOrEmpty();
            defaultCategories.Count().Should().Be(3);
            defaultCategories
                .Where(dc => string.IsNullOrEmpty(dc.Name))
                .Should().BeNullOrEmpty();
        }
    }
}
