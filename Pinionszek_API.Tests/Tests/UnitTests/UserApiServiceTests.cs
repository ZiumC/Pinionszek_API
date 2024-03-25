using FluentAssertions;
using Pinionszek_API.Services.DatabaseServices.BudgetService;
using Pinionszek_API.Services.DatabaseServices.UserService;
using Pinionszek_API.Tests.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinionszek_API.Tests.Tests.UnitTests
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
    }
}
