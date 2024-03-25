using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pinionszek_API.Controllers;
using Pinionszek_API.Models.DTOs.GetDto.Payments;
using Pinionszek_API.Models.DTOs.GetDto.User;
using Pinionszek_API.Profiles;
using Pinionszek_API.Services.DatabaseServices.BudgetService;
using Pinionszek_API.Services.DatabaseServices.UserService;
using Pinionszek_API.Tests.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinionszek_API.Tests.Tests.IntegrationTests
{
    public class UserControllerTests
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public UserControllerTests()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                     path: "appsettings.json",
                     optional: false,
                     reloadOnChange: true)
               .Build();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfile());
            });
            _mapper = mockMapper.CreateMapper();
        }

        [Fact]
        public async Task UserController_GetUserFriendsAsync_ReturnsFriendssOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var userApiService = new UserApiService(await dbContext);
            var userController = new UserController(_config, userApiService, _mapper);
            int user_1 = 1;
            int user_2 = 2;
            int user_3 = 3;
            int user_4 = 4;

            //Act
            var okRequest_1 = await userController.GetUserFriendsAsync(user_1);
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var friendsResult_1 = okActionResult_1?.Value as IEnumerable<GetUserFriendDto>;

            var okRequest_2 = await userController.GetUserFriendsAsync(user_2);
            var okActionResult_2 = okRequest_2 as OkObjectResult;
            var friendsResult_2 = okActionResult_2?.Value as IEnumerable<GetUserFriendDto>;

            var okRequest_3 = await userController.GetUserFriendsAsync(user_3);
            var okActionResult_3 = okRequest_3 as OkObjectResult;
            var friendsResult_3 = okActionResult_3?.Value as IEnumerable<GetUserFriendDto>;

            var notfound_1 = await userController.GetUserFriendsAsync(user_4);

            var badRequest_1 = await userController.GetUserFriendsAsync(-user_4);
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            okActionResult_1.Should().NotBeNull();
            friendsResult_1?.Count().Should().Be(3);
            friendsResult_1?
                .Where(fr => fr.IdFriend <= 0 || fr.FriendTag <= 0)
                .Should().BeNullOrEmpty();
            friendsResult_1?
                .Where(fr => string.IsNullOrEmpty(fr.Login))
                .Should().BeNullOrEmpty();

            okRequest_2.Should().BeOfType<OkObjectResult>();
            okActionResult_2.Should().NotBeNull();
            friendsResult_2?.Count().Should().Be(2);
            friendsResult_2?
                .Where(fr => fr.IdFriend <= 0 || fr.FriendTag <= 0)
                .Should().BeNullOrEmpty();
            friendsResult_2?
                .Where(fr => string.IsNullOrEmpty(fr.Login))
                .Should().BeNullOrEmpty();

            okRequest_3.Should().BeOfType<OkObjectResult>();
            okActionResult_3.Should().NotBeNull();
            friendsResult_3?.Count().Should().Be(1);
            friendsResult_3?
                .Where(fr => fr.IdFriend <= 0 || fr.FriendTag <= 0)
                .Should().BeNullOrEmpty();
            friendsResult_3?
                .Where(fr => string.IsNullOrEmpty(fr.Login))
                .Should().BeNullOrEmpty();

            notfound_1.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_1?.Value.Should().NotBeNull();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();
        }
    }
}
