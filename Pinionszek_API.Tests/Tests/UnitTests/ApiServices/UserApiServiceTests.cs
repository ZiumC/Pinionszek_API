﻿using FluentAssertions;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Services.DatabaseServices.UserService;
using Pinionszek_API.Tests.DbContexts;

namespace Pinionszek_API.Tests.Tests.UnitTests.ApiServices
{
    public class UserApiServiceTests
    {
        private readonly int _user_1 = 1;
        private readonly int _user_2 = 2;
        private readonly int _user_3 = 3;
        private readonly int _user_4 = 4;
        private readonly int _user_5 = 100;
        private readonly DateTime _registrationDate = DateTime.Parse("2019-01-01");

        [Fact]
        public async Task UserApiService_GetUserFriendsAsync_ReturnsFriendsOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var userApiService = new UserApiService(await dbContext);
            var user_1 = new { IdUser = 1, UserTag = 1001 };
            var user_2 = new { IdUser = 2, UserTag = 1002 };
            var user_3 = new { IdUser = 3, UserTag = 1003 };
            var user_4 = new { IdUser = 4, UserTag = 1004 };

            //Act
            var userFriends_1 = await userApiService.GetUserFriendsAsync(user_1.IdUser);
            var userFriends_2 = await userApiService.GetUserFriendsAsync(user_2.IdUser);
            var userFriends_3 = await userApiService.GetUserFriendsAsync(user_3.IdUser);
            var userFriends_4 = await userApiService.GetUserFriendsAsync(user_4.IdUser);

            //Assert
            userFriends_1.Should().NotBeNull();
            userFriends_1?.Count().Should().Be(3);
            userFriends_1?
                .Where(uf => string.IsNullOrEmpty(uf.User.Login))
                .Should().BeNullOrEmpty();
            userFriends_1?
                .Where(uf => uf.FriendTag == user_1.UserTag)
                .Should().BeNullOrEmpty();

            userFriends_2.Should().NotBeNull();
            userFriends_2?.Count().Should().Be(2);
            userFriends_2?
                .Where(uf => string.IsNullOrEmpty(uf.User.Login))
                .Should().BeNullOrEmpty();
            userFriends_2?
                .Where(uf => uf.FriendTag == user_2.UserTag)
                .Should().BeNullOrEmpty();

            userFriends_3.Should().NotBeNull();
            userFriends_3?.Count().Should().Be(1);
            userFriends_3?
                .Where(uf => string.IsNullOrEmpty(uf.User.Login))
                .Should().BeNullOrEmpty();
            userFriends_3?
                .Where(uf => uf.FriendTag == user_3.UserTag)
                .Should().BeNullOrEmpty();

            userFriends_4.Should().BeNullOrEmpty();
        }
        [Fact]
        public async Task UserApiService_GetUserSettingsAsync_ReturnsSettingsOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var userApiService = new UserApiService(await dbContext);

            //Act
            var userSettings_1 = await userApiService.GetUserSettingsAsync(_user_1);
            var userSettings_2 = await userApiService.GetUserSettingsAsync(_user_2);
            var userSettings_3 = await userApiService.GetUserSettingsAsync(_user_3);
            var userSettings_4 = await userApiService.GetUserSettingsAsync(_user_4);
            var userSettings_5 = await userApiService.GetUserSettingsAsync(_user_5);

            //Assert
            userSettings_1.Should().NotBeNull();

            userSettings_2.Should().NotBeNull();

            userSettings_3.Should().NotBeNull();

            userSettings_4.Should().NotBeNull();

            userSettings_5.Should().BeNull();
        }
        [Fact]
        public async Task UserApiService_GetUserDataAsync_ReturnsUserOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var userApiService = new UserApiService(await dbContext);

            //Act
            var userData_1 = await userApiService.GetUserDataAsync(_user_1);
            var userData_2 = await userApiService.GetUserDataAsync(_user_2);
            var userData_3 = await userApiService.GetUserDataAsync(_user_3);
            var userData_4 = await userApiService.GetUserDataAsync(_user_4);
            var userData_5 = await userApiService.GetUserDataAsync(_user_5);

            //Assert
            userData_1.Should().NotBeNull();
            userData_1?.IdUser.Should().BeGreaterThan(0);
            userData_1?.UserTag.Should().BeGreaterThan(0);
            userData_1?.Email.Should().NotBeNullOrEmpty();
            userData_1?.Login.Should().NotBeNullOrEmpty();
            userData_1?.Password.Should().NotBeNullOrEmpty();
            userData_1?.PasswordSalt.Should().NotBeNullOrEmpty();
            userData_1?.RegisteredAt.Should().BeAfter(_registrationDate);

            userData_2.Should().NotBeNull();
            userData_2?.IdUser.Should().BeGreaterThan(0);
            userData_2?.UserTag.Should().BeGreaterThan(0);
            userData_2?.Email.Should().NotBeNullOrEmpty();
            userData_2?.Login.Should().NotBeNullOrEmpty();
            userData_2?.Password.Should().NotBeNullOrEmpty();
            userData_2?.PasswordSalt.Should().NotBeNullOrEmpty();
            userData_2?.RegisteredAt.Should().BeAfter(_registrationDate);

            userData_3.Should().NotBeNull();
            userData_3?.IdUser.Should().BeGreaterThan(0);
            userData_3?.UserTag.Should().BeGreaterThan(0);
            userData_3?.Email.Should().NotBeNullOrEmpty();
            userData_3?.Login.Should().NotBeNullOrEmpty();
            userData_3?.Password.Should().NotBeNullOrEmpty();
            userData_3?.PasswordSalt.Should().NotBeNullOrEmpty();
            userData_3?.RegisteredAt.Should().BeAfter(_registrationDate);

            userData_4.Should().NotBeNull();
            userData_4?.IdUser.Should().BeGreaterThan(0);
            userData_4?.UserTag.Should().BeGreaterThan(0);
            userData_4?.Email.Should().NotBeNullOrEmpty();
            userData_4?.Login.Should().NotBeNullOrEmpty();
            userData_4?.Password.Should().NotBeNullOrEmpty();
            userData_4?.PasswordSalt.Should().NotBeNullOrEmpty();
            userData_4?.RegisteredAt.Should().BeAfter(_registrationDate);

            userData_5.Should().BeNull();
        }

        [Fact]
        public async Task UserApiService_GetUserPaymentCategoriesAsync_ReturnsCategoriesOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var userApiService = new UserApiService(await dbContext);

            //Act
            var userCategories1 = await userApiService.GetUserPaymentCategoriesAsync(_user_1);
            var userCategories2 = await userApiService.GetUserPaymentCategoriesAsync(_user_2);
            var userCategories3 = await userApiService.GetUserPaymentCategoriesAsync(_user_3);
            var userCategories4 = await userApiService.GetUserPaymentCategoriesAsync(_user_4);

            //Assert
            userCategories1.Should().NotBeNullOrEmpty();
            userCategories1?.Count().Should().Be(6);
            userCategories1?
                .Where(uc => string.IsNullOrEmpty(uc.Name) ||
                    string.IsNullOrEmpty(uc.GeneralCategory.Name))
                .Should().BeNullOrEmpty();
            userCategories1?
                .Where(uc => uc.IdDetailedCategory <= 0 ||
                    uc.GeneralCategory.IdGeneralCategory <= 0)
                .Should().BeNullOrEmpty();

            userCategories2.Should().NotBeNullOrEmpty();
            userCategories2?.Count().Should().Be(4);
            userCategories2?
                .Where(uc => string.IsNullOrEmpty(uc.Name) ||
                    string.IsNullOrEmpty(uc.GeneralCategory.Name))
                .Should().BeNullOrEmpty();
            userCategories2?
                .Where(uc => uc.IdDetailedCategory <= 0 ||
                    uc.GeneralCategory.IdGeneralCategory <= 0)
                .Should().BeNullOrEmpty();

            userCategories3.Should().NotBeNullOrEmpty();
            userCategories3?.Count().Should().Be(3);
            userCategories3?
                .Where(uc => string.IsNullOrEmpty(uc.Name) ||
                    string.IsNullOrEmpty(uc.GeneralCategory.Name))
                .Should().BeNullOrEmpty();
            userCategories3?
                .Where(uc => uc.IdDetailedCategory <= 0 ||
                    uc.GeneralCategory.IdGeneralCategory <= 0)
                .Should().BeNullOrEmpty();

            userCategories4.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task UserApiService_GetUserPaymentCategoryAsync_ReturnsCategoryOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var userApiService = new UserApiService(await dbContext);
            var user_1 = new { IdUser = 1, IdDetailedCategories = new List<int>() { 1, 3, 4, 5, 8, 9 } };
            var user_2 = new { IdUser = 2, IdDetailedCategories = new List<int>() { 2, 6, 10, 12 } };
            var user_3 = new { IdUser = 3, IdDetailedCategories = new List<int>() { 7, 11, 13 } };
            var user_4 = new { IdUser = 4, IdDetailedCategories = new List<int>() { 1, 11, 29 } };
            var userCategories_1 = new List<DetailedCategory?>();
            var userCategories_2 = new List<DetailedCategory?>();
            var userCategories_3 = new List<DetailedCategory?>();
            var userCategories_4 = new List<DetailedCategory?>();

            //Act
            foreach (int idDetaieldCategory in user_1.IdDetailedCategories)
            {
                userCategories_1.Add(await userApiService.GetUserPaymentCategoryAsync(user_1.IdUser, idDetaieldCategory));
            }

            foreach (int idDetaieldCategory in user_2.IdDetailedCategories)
            {
                userCategories_2.Add(await userApiService.GetUserPaymentCategoryAsync(user_2.IdUser, idDetaieldCategory));
            }

            foreach (int idDetaieldCategory in user_3.IdDetailedCategories)
            {
                userCategories_3.Add(await userApiService.GetUserPaymentCategoryAsync(user_3.IdUser, idDetaieldCategory));
            }

            foreach (int idDetaieldCategory in user_4.IdDetailedCategories)
            {
                userCategories_4.Add(await userApiService.GetUserPaymentCategoryAsync(user_4.IdUser, idDetaieldCategory));
            }

            //Assert
            userCategories_1.Should().NotBeNullOrEmpty();
            userCategories_1?.Count().Should().Be(6);
            userCategories_1?
                .Where(uc => string.IsNullOrEmpty(uc?.Name) ||
                    string.IsNullOrEmpty(uc.GeneralCategory.Name))
                .Should().BeNullOrEmpty();
            userCategories_1?
                .Where(uc => uc?.IdDetailedCategory <= 0 ||
                    uc?.GeneralCategory.IdGeneralCategory <= 0)
                .Should().BeNullOrEmpty();

            userCategories_2.Should().NotBeNullOrEmpty();
            userCategories_2?.Count().Should().Be(4);
            userCategories_2?
                .Where(uc => string.IsNullOrEmpty(uc?.Name) ||
                    string.IsNullOrEmpty(uc?.GeneralCategory.Name))
                .Should().BeNullOrEmpty();
            userCategories_2?
                .Where(uc => uc?.IdDetailedCategory <= 0 ||
                    uc?.GeneralCategory.IdGeneralCategory <= 0)
                .Should().BeNullOrEmpty();

            userCategories_3.Should().NotBeNullOrEmpty();
            userCategories_3?.Count().Should().Be(3);
            userCategories_3?
                .Where(uc => string.IsNullOrEmpty(uc?.Name) ||
                    string.IsNullOrEmpty(uc?.GeneralCategory.Name))
                .Should().BeNullOrEmpty();
            userCategories_3?
                .Where(uc => uc?.IdDetailedCategory <= 0 ||
                    uc?.GeneralCategory.IdGeneralCategory <= 0)
                .Should().BeNullOrEmpty();

            userCategories_4.Should().NotBeNullOrEmpty();
            userCategories_4?.Count().Should().Be(3);
            foreach (var cat in userCategories_4)
            {
                cat.Should().BeNull();
            }
        }
    }
}
