using FluentAssertions;
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
    public class UserApiServiceTests
    {

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
            int user_1 = 1;
            int user_2 = 2;
            int user_3 = 3;
            int user_4 = 4;
            int user_5 = 5;

            //Act
            var userSettings_1 = await userApiService.GetUserSettingsAsync(user_1);
            var userSettings_2 = await userApiService.GetUserSettingsAsync(user_2);
            var userSettings_3 = await userApiService.GetUserSettingsAsync(user_3);
            var userSettings_4 = await userApiService.GetUserSettingsAsync(user_4);
            var userSettings_5 = await userApiService.GetUserSettingsAsync(user_5);

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
            int user_1 = 1;
            int user_2 = 2;
            int user_3 = 3;
            int user_4 = 4;
            int user_5 = 5;

            //Act
            var userData_1 = await userApiService.GetUserDataAsync(user_1);
            var userData_2 = await userApiService.GetUserDataAsync(user_2);
            var userData_3 = await userApiService.GetUserDataAsync(user_3);
            var userData_4 = await userApiService.GetUserDataAsync(user_4);
            var userData_5 = await userApiService.GetUserDataAsync(user_5);

            //Assert
            userData_1.Should().NotBeNull();
            userData_1?.IdUser.Should().BeGreaterThan(0);
            userData_1?.UserTag.Should().BeGreaterThan(0);
            userData_1?.Email.Should().NotBeNullOrEmpty();
            userData_1?.Login.Should().NotBeNullOrEmpty();
            userData_1?.Password.Should().NotBeNullOrEmpty();
            userData_1?.PasswordSalt.Should().NotBeNullOrEmpty();
            userData_1?.RegisteredAt.Should().BeAfter(DateTime.Parse("2019-01-01"));

            userData_2.Should().NotBeNull();
            userData_2?.IdUser.Should().BeGreaterThan(0);
            userData_2?.UserTag.Should().BeGreaterThan(0);
            userData_2?.Email.Should().NotBeNullOrEmpty();
            userData_2?.Login.Should().NotBeNullOrEmpty();
            userData_2?.Password.Should().NotBeNullOrEmpty();
            userData_2?.PasswordSalt.Should().NotBeNullOrEmpty();
            userData_2?.RegisteredAt.Should().BeAfter(DateTime.Parse("2019-01-01"));

            userData_3.Should().NotBeNull();
            userData_3?.IdUser.Should().BeGreaterThan(0);
            userData_3?.UserTag.Should().BeGreaterThan(0);
            userData_3?.Email.Should().NotBeNullOrEmpty();
            userData_3?.Login.Should().NotBeNullOrEmpty();
            userData_3?.Password.Should().NotBeNullOrEmpty();
            userData_3?.PasswordSalt.Should().NotBeNullOrEmpty();
            userData_3?.RegisteredAt.Should().BeAfter(DateTime.Parse("2019-01-01"));

            userData_4.Should().NotBeNull();
            userData_4?.IdUser.Should().BeGreaterThan(0);
            userData_4?.UserTag.Should().BeGreaterThan(0);
            userData_4?.Email.Should().NotBeNullOrEmpty();
            userData_4?.Login.Should().NotBeNullOrEmpty();
            userData_4?.Password.Should().NotBeNullOrEmpty();
            userData_4?.PasswordSalt.Should().NotBeNullOrEmpty();
            userData_4?.RegisteredAt.Should().BeAfter(DateTime.Parse("2019-01-01"));

            userData_5.Should().BeNull();
        }

        [Fact]
        public async Task UserApiService_GetUserPaymentCategoriesAsync_ReturnsCategoriesOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var userApiService = new UserApiService(await dbContext);
            int idUser_1 = 1;
            int idUser_2 = 2;
            int idUser_3 = 3;
            int idUser_4 = 4;

            //Act
            var userCategories1 = await userApiService.GetUserPaymentCategoriesAsync(idUser_1);
            var userCategories2 = await userApiService.GetUserPaymentCategoriesAsync(idUser_2);
            var userCategories3 = await userApiService.GetUserPaymentCategoriesAsync(idUser_3);
            var userCategories4 = await userApiService.GetUserPaymentCategoriesAsync(idUser_4);

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
    }
}
