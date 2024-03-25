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

        [Fact]
        public async Task UserController_GetUserSettingsAsync_ReturnsSettingsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var userApiService = new UserApiService(await dbContext);
            var userController = new UserController(_config, userApiService, _mapper);
            int user_1 = 1;
            int user_2 = 2;
            int user_3 = 3;
            int user_4 = 4;
            int user_5 = 5;

            //Act
            var okRequest_1 = await userController.GetUserSettingsAsync(user_1);
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var settingsResult_1 = okActionResult_1?.Value as GetUserSettingsDto;

            var okRequest_2 = await userController.GetUserSettingsAsync(user_2);
            var okActionResult_2 = okRequest_2 as OkObjectResult;
            var settingsResult_2 = okActionResult_2?.Value as GetUserSettingsDto;

            var okRequest_3 = await userController.GetUserSettingsAsync(user_3);
            var okActionResult_3 = okRequest_3 as OkObjectResult;
            var settingsResult_3 = okActionResult_3?.Value as GetUserSettingsDto;

            var okRequest_4 = await userController.GetUserSettingsAsync(user_4);
            var okActionResult_4 = okRequest_4 as OkObjectResult;
            var settingsResult_4 = okActionResult_4?.Value as GetUserSettingsDto;

            var notfound_1 = await userController.GetUserSettingsAsync(user_5);

            var badRequest_1 = await userController.GetUserSettingsAsync(-user_4);
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            okActionResult_1.Should().NotBeNull();
            settingsResult_1?.UseBudgetRules.Should().BeTrue();
            settingsResult_1?.DisplayBudgetRules.Should().BeFalse();
            settingsResult_1?.IdUserSetting.Should().BeGreaterThan(0);
            settingsResult_1?.NeedsRule.Should().BeGreaterThanOrEqualTo(0);
            settingsResult_1?.WantsRule.Should().BeGreaterThanOrEqualTo(0);
            settingsResult_1?.SavingsRule.Should().BeGreaterThanOrEqualTo(0);
            var percentSum_1 = settingsResult_1?.NeedsRule + settingsResult_1?.WantsRule +
                settingsResult_1?.SavingsRule;
            percentSum_1.Should().BeLessThanOrEqualTo(100);

            okRequest_2.Should().BeOfType<OkObjectResult>();
            okActionResult_2.Should().NotBeNull();
            settingsResult_2?.UseBudgetRules.Should().BeFalse();
            settingsResult_2?.DisplayBudgetRules.Should().BeFalse();
            settingsResult_2?.IdUserSetting.Should().BeGreaterThan(0);
            settingsResult_2?.NeedsRule.Should().BeGreaterThanOrEqualTo(0);
            settingsResult_2?.WantsRule.Should().BeGreaterThanOrEqualTo(0);
            settingsResult_2?.SavingsRule.Should().BeGreaterThanOrEqualTo(0);
            var percentSum_2 = settingsResult_2?.NeedsRule + settingsResult_2?.WantsRule +
                settingsResult_2?.SavingsRule;
            percentSum_2.Should().BeLessThanOrEqualTo(100);

            okRequest_3.Should().BeOfType<OkObjectResult>();
            okActionResult_3.Should().NotBeNull();
            settingsResult_3?.UseBudgetRules.Should().BeTrue();
            settingsResult_3?.DisplayBudgetRules.Should().BeTrue();
            settingsResult_3?.IdUserSetting.Should().BeGreaterThan(0);
            settingsResult_3?.NeedsRule.Should().BeGreaterThanOrEqualTo(0);
            settingsResult_3?.WantsRule.Should().BeGreaterThanOrEqualTo(0);
            settingsResult_3?.SavingsRule.Should().BeGreaterThanOrEqualTo(0);
            var percentSum_3 = settingsResult_3?.NeedsRule + settingsResult_3?.WantsRule +
                settingsResult_3?.SavingsRule;
            percentSum_3.Should().BeLessThanOrEqualTo(100);

            okRequest_4.Should().BeOfType<OkObjectResult>();
            okActionResult_4.Should().NotBeNull();
            settingsResult_4?.UseBudgetRules.Should().BeTrue();
            settingsResult_4?.DisplayBudgetRules.Should().BeTrue();
            settingsResult_4?.IdUserSetting.Should().BeGreaterThan(0);
            settingsResult_4?.NeedsRule.Should().BeGreaterThanOrEqualTo(0);
            settingsResult_4?.WantsRule.Should().BeGreaterThanOrEqualTo(0);
            settingsResult_4?.SavingsRule.Should().BeGreaterThanOrEqualTo(0);
            var percentSum_4 = settingsResult_4?.NeedsRule + settingsResult_4?.WantsRule +
                settingsResult_4?.SavingsRule;
            percentSum_4.Should().BeLessThanOrEqualTo(100);

            notfound_1.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_1?.Value.Should().NotBeNull();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();
        }

        [Fact]
        public async Task UserControllerGetUserProfileDataAsync_ReturnsUserProfileOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var userApiService = new UserApiService(await dbContext);
            var userController = new UserController(_config, userApiService, _mapper);
            int user_1 = 1;
            int user_2 = 2;
            int user_3 = 3;
            int user_4 = 4;
            int user_5 = 5;
            var dateAfter = DateTime.Parse("2019-01-01");

            //Act
            var okRequest_1 = await userController.GetUserProfileDataAsync(user_1);
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var profileResult_1 = okActionResult_1?.Value as GetUserProfileDto;

            var okRequest_2 = await userController.GetUserProfileDataAsync(user_2);
            var okActionResult_2 = okRequest_2 as OkObjectResult;
            var profileResult_2 = okActionResult_2?.Value as GetUserProfileDto;

            var okRequest_3 = await userController.GetUserProfileDataAsync(user_3);
            var okActionResult_3 = okRequest_3 as OkObjectResult;
            var profileResult_3 = okActionResult_3?.Value as GetUserProfileDto;

            var okRequest_4 = await userController.GetUserProfileDataAsync(user_4);
            var okActionResult_4 = okRequest_4 as OkObjectResult;
            var profileResult_4 = okActionResult_4?.Value as GetUserProfileDto;

            var notfound_1 = await userController.GetUserProfileDataAsync(user_5);

            var badRequest_1 = await userController.GetUserProfileDataAsync(-user_4);
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            okActionResult_1.Should().NotBeNull();
            profileResult_1?.UserTag.Should().BeGreaterThan(0);
            profileResult_1?.RegisteredAt.Should().BeAfter(dateAfter);
            profileResult_1?.Email.Should().Contain("*");

            okRequest_2.Should().BeOfType<OkObjectResult>();
            okActionResult_2.Should().NotBeNull();
            profileResult_2?.UserTag.Should().BeGreaterThan(0);
            profileResult_2?.RegisteredAt.Should().BeAfter(dateAfter);
            profileResult_2?.Email.Should().Contain("*");

            okRequest_3.Should().BeOfType<OkObjectResult>();
            okActionResult_3.Should().NotBeNull();
            profileResult_3?.UserTag.Should().BeGreaterThan(0);
            profileResult_3?.RegisteredAt.Should().BeAfter(dateAfter);
            profileResult_3?.Email.Should().Contain("*");

            okRequest_4.Should().BeOfType<OkObjectResult>();
            okActionResult_4.Should().NotBeNull();
            profileResult_4?.UserTag.Should().BeGreaterThan(0);
            profileResult_4?.RegisteredAt.Should().BeAfter(dateAfter);
            profileResult_4?.Email.Should().Contain("*");

            notfound_1.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_1?.Value.Should().NotBeNull();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();
        }
    }
}
