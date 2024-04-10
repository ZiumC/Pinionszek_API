using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pinionszek_API.Controllers;
using Pinionszek_API.Models.DatabaseModel;
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
        private readonly int _user_1 = 1;
        private readonly int _user_2 = 2;
        private readonly int _user_3 = 3;
        private readonly int _user_4 = 4;
        private readonly int _user_5 = 100;
        private readonly int _defaultPageSize = 1;
        private readonly DateTime _budgetDate;

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
                cfg.AddProfile(new CategoryProfile());
            });
            _mapper = mockMapper.CreateMapper();
            _budgetDate = DateTime.Parse("2024-01-01");
        }

        [Fact]
        public async Task UserController_GetUserFriendsAsync_ReturnsFriendssOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var userApiService = new UserApiService(await dbContext);
            var userController = new UserController(_config, userApiService, _mapper);
            var pages = new { page0 = 0, page1 = 1, page2 = 2 };
            var pageSize = 2;

            //Act
            var okRequest_1 = await userController.GetUserFriendsAsync(_user_1);
            var friendsResult_1 = (okRequest_1 as OkObjectResult)?.Value as IEnumerable<GetUserFriendDto>;

            var okRequest_2 = await userController.GetUserFriendsAsync(_user_2);
            var friendsResult_2 = (okRequest_2 as OkObjectResult)?.Value as IEnumerable<GetUserFriendDto>;

            var okRequest_3 = await userController.GetUserFriendsAsync(_user_3);
            var okActionResult_3 = okRequest_3 as OkObjectResult;
            var friendsResult_3 = okActionResult_3?.Value as IEnumerable<GetUserFriendDto>;

            var okRequestPage_1 = await userController.GetUserFriendsAsync
                (_user_1, pages.page1, pageSize);
            var friendsResultPage_1 = (okRequestPage_1 as OkObjectResult)?.Value as IEnumerable<GetUserFriendDto>;

            var okRequestPage_2 = await userController.GetUserFriendsAsync
                (_user_1, pages.page2, pageSize);
            var friendsResultPage_2 = (okRequestPage_2 as OkObjectResult)?.Value as IEnumerable<GetUserFriendDto>;

            var notfound_1 = await userController.GetUserFriendsAsync(_user_4);

            var badRequest_1 = await userController.GetUserFriendsAsync(-_user_4);
            var badRequestResult_1 = (badRequest_1 as BadRequestObjectResult)?.Value as string;

            var badRequest_2 = await userController.GetUserFriendsAsync
                (_user_4, pages.page0, _defaultPageSize);
            var badRequestResult_2 = (badRequest_2 as BadRequestObjectResult)?.Value as string;

            var badRequest_3 = await userController.GetUserFriendsAsync
                (_user_4, pages.page1, -_defaultPageSize);
            var badRequestResult_3 = (badRequest_3 as BadRequestObjectResult)?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            friendsResult_1?.Count().Should().Be(3);
            friendsResult_1?
                .Where(fr => fr.IdFriend <= 0 || fr.FriendTag <= 0)
                .Should().BeNullOrEmpty();
            friendsResult_1?
                .Where(fr => string.IsNullOrEmpty(fr.Login))
                .Should().BeNullOrEmpty();

            okRequest_2.Should().BeOfType<OkObjectResult>();
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

            okRequestPage_1.Should().BeOfType<OkObjectResult>();
            friendsResultPage_1?.Count().Should().Be(2);
            friendsResultPage_1?
                .Where(fr => fr.IdFriend <= 0 || fr.FriendTag <= 0)
                .Should().BeNullOrEmpty();
            friendsResultPage_1?
                .Where(fr => string.IsNullOrEmpty(fr.Login))
                .Should().BeNullOrEmpty();

            okRequestPage_2.Should().BeOfType<OkObjectResult>();
            friendsResultPage_2?.Count().Should().Be(1);
            friendsResultPage_2?
                .Where(fr => fr.IdFriend <= 0 || fr.FriendTag <= 0)
                .Should().BeNullOrEmpty();
            friendsResultPage_2?
                .Where(fr => string.IsNullOrEmpty(fr.Login))
                .Should().BeNullOrEmpty();

            notfound_1.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_2?.Contains("is invalid").Should().BeTrue();

            badRequest_3.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_3?.Contains("is invalid").Should().BeTrue();
        }

        [Fact]
        public async Task UserController_GetUserSettingsAsync_ReturnsSettingsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var userApiService = new UserApiService(await dbContext);
            var userController = new UserController(_config, userApiService, _mapper);

            //Act
            var okRequest_1 = await userController.GetUserSettingsAsync(_user_1);
            var settingsResult_1 = (okRequest_1 as OkObjectResult)?.Value as GetUserSettingsDto;

            var okRequest_2 = await userController.GetUserSettingsAsync(_user_2);
            var settingsResult_2 = (okRequest_2 as OkObjectResult)?.Value as GetUserSettingsDto;

            var okRequest_3 = await userController.GetUserSettingsAsync(_user_3);
            var settingsResult_3 = (okRequest_3 as OkObjectResult)?.Value as GetUserSettingsDto;

            var okRequest_4 = await userController.GetUserSettingsAsync(_user_4);
            var settingsResult_4 = (okRequest_4 as OkObjectResult)?.Value as GetUserSettingsDto;

            var notfound_1 = await userController.GetUserSettingsAsync(_user_5);

            var badRequest_1 = await userController.GetUserSettingsAsync(-_user_4);
            var badRequestResult_1 = (badRequest_1 as BadRequestObjectResult)?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
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
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();
        }

        [Fact]
        public async Task UserControllerGetUserProfileDataAsync_ReturnsUserProfileOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var userApiService = new UserApiService(await dbContext);
            var userController = new UserController(_config, userApiService, _mapper);
            var dateAfter = DateTime.Parse("2019-01-01");

            //Act
            var okRequest_1 = await userController.GetUserProfileDataAsync(_user_1);
            var profileResult_1 = (okRequest_1 as OkObjectResult)?.Value as GetUserProfileDto;

            var okRequest_2 = await userController.GetUserProfileDataAsync(_user_2);
            var profileResult_2 = (okRequest_2 as OkObjectResult)?.Value as GetUserProfileDto;

            var okRequest_3 = await userController.GetUserProfileDataAsync(_user_3);
            var profileResult_3 = (okRequest_3 as OkObjectResult)?.Value as GetUserProfileDto;

            var okRequest_4 = await userController.GetUserProfileDataAsync(_user_4);
            var profileResult_4 = (okRequest_4 as OkObjectResult)?.Value as GetUserProfileDto;

            var notfound_1 = await userController.GetUserProfileDataAsync(_user_5);

            var badRequest_1 = await userController.GetUserProfileDataAsync(-_user_4);
            var badRequestResult_1 = (badRequest_1 as BadRequestObjectResult)?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            profileResult_1?.UserTag.Should().BeGreaterThan(0);
            profileResult_1?.RegisteredAt.Should().BeAfter(dateAfter);
            profileResult_1?.Email.Should().Contain("*");

            okRequest_2.Should().BeOfType<OkObjectResult>();
            profileResult_2?.UserTag.Should().BeGreaterThan(0);
            profileResult_2?.RegisteredAt.Should().BeAfter(dateAfter);
            profileResult_2?.Email.Should().Contain("*");

            okRequest_3.Should().BeOfType<OkObjectResult>();
            profileResult_3?.UserTag.Should().BeGreaterThan(0);
            profileResult_3?.RegisteredAt.Should().BeAfter(dateAfter);
            profileResult_3?.Email.Should().Contain("*");

            okRequest_4.Should().BeOfType<OkObjectResult>();
            profileResult_4?.UserTag.Should().BeGreaterThan(0);
            profileResult_4?.RegisteredAt.Should().BeAfter(dateAfter);
            profileResult_4?.Email.Should().Contain("*");

            notfound_1.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();
        }

        [Fact]
        public async Task UserController_GetPaymentsCategoriesAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var userApiService = new UserApiService(await dbContext);
            var userController = new UserController(_config, userApiService, _mapper);
            var pages = new { page0 = 0, page1 = 1, page2 = 2 };
            int pageSize = 4;

            //Act
            var okRequest_1 = await userController.GetPaymentsCategoriesAsync(_user_1);
            var paymentsCategoriesResult_1 = (okRequest_1 as OkObjectResult)?.Value as IEnumerable<GetUserCategoryDto>;

            var okRequest_2 = await userController.GetPaymentsCategoriesAsync(_user_2);
            var paymentsCategoriesResult_2 = (okRequest_2 as OkObjectResult)?.Value as IEnumerable<GetUserCategoryDto>;

            var okRequest_3 = await userController.GetPaymentsCategoriesAsync(_user_3);
            var paymentsCategoriesResult_3 = (okRequest_3 as OkObjectResult)?.Value as IEnumerable<GetUserCategoryDto>;

            var okRequestPage_1 = await userController.GetPaymentsCategoriesAsync
                (_user_1, pages.page1, pageSize);
            var paymentsCatResultPage_1 = (okRequestPage_1 as OkObjectResult)?.Value as IEnumerable<GetUserCategoryDto>;

            var okRequestPage_2 = await userController.GetPaymentsCategoriesAsync
                (_user_1, pages.page2, pageSize);
            var paymentsCatResultPage_2 = (okRequestPage_2 as OkObjectResult)?.Value as IEnumerable<GetUserCategoryDto>;


            var notFoundRequest_1 = await userController.GetPaymentsCategoriesAsync(_user_4);

            var badRequest_1 = await userController.GetPaymentsCategoriesAsync(-_user_1);
            var badRequestResult_1 = (badRequest_1 as BadRequestObjectResult)?.Value as string;

            var badRequest_2 = await userController.GetPaymentsCategoriesAsync
                (_user_1, pages.page0, _defaultPageSize);
            var badRequestResult_2 = (badRequest_2 as BadRequestObjectResult)?.Value as string;

            var badRequest_3 = await userController.GetPaymentsCategoriesAsync
                (_user_1, pages.page1, -_defaultPageSize);
            var badRequestResult_3 = (badRequest_3 as BadRequestObjectResult)?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            paymentsCategoriesResult_1.Should().NotBeNullOrEmpty();
            paymentsCategoriesResult_1?.Count().Should().Be(6);
            paymentsCategoriesResult_1?
                .Where(pcr => pcr.IdDetailedCategory <= 0 ||
                        pcr.GeneralCategory.IdGeneralCategory <= 0)
                .Should().BeNullOrEmpty();
            paymentsCategoriesResult_1?
                .Where(pcr => string.IsNullOrEmpty(pcr.Name) ||
                string.IsNullOrEmpty(pcr.GeneralCategory.Name))
                .Should().BeNullOrEmpty();

            okRequest_2.Should().BeOfType<OkObjectResult>();
            paymentsCategoriesResult_2.Should().NotBeNullOrEmpty();
            paymentsCategoriesResult_2?.Count().Should().Be(4);
            paymentsCategoriesResult_2?
                .Where(pcr => pcr.IdDetailedCategory <= 0 ||
                        pcr.GeneralCategory.IdGeneralCategory <= 0)
                .Should().BeNullOrEmpty();
            paymentsCategoriesResult_2?
                .Where(pcr => string.IsNullOrEmpty(pcr.Name) ||
                    string.IsNullOrEmpty(pcr.GeneralCategory.Name))
                .Should().BeNullOrEmpty();

            okRequest_3.Should().BeOfType<OkObjectResult>();
            paymentsCategoriesResult_3.Should().NotBeNullOrEmpty();
            paymentsCategoriesResult_3?.Count().Should().Be(3);
            paymentsCategoriesResult_3?
                .Where(pcr => pcr.IdDetailedCategory <= 0 ||
                    pcr.GeneralCategory.IdGeneralCategory <= 0)
                .Should().BeNullOrEmpty();
            paymentsCategoriesResult_3?
                .Where(pcr => string.IsNullOrEmpty(pcr.Name) ||
                    string.IsNullOrEmpty(pcr.GeneralCategory.Name))
                .Should().BeNullOrEmpty();

            okRequestPage_1.Should().BeOfType<OkObjectResult>();
            paymentsCatResultPage_1.Should().NotBeNullOrEmpty();
            paymentsCatResultPage_1?.Count().Should().Be(4);
            paymentsCatResultPage_1?
                .Where(pcr => pcr.IdDetailedCategory <= 0 ||
                    pcr.GeneralCategory.IdGeneralCategory <= 0)
                .Should().BeNullOrEmpty();
            paymentsCatResultPage_1?
                .Where(pcr => string.IsNullOrEmpty(pcr.Name) ||
                    string.IsNullOrEmpty(pcr.GeneralCategory.Name))
                .Should().BeNullOrEmpty();

            okRequestPage_2.Should().BeOfType<OkObjectResult>();
            paymentsCatResultPage_2.Should().NotBeNullOrEmpty();
            paymentsCatResultPage_2?.Count().Should().Be(2);
            paymentsCatResultPage_2?
                .Where(pcr => pcr.IdDetailedCategory <= 0 ||
                    pcr.GeneralCategory.IdGeneralCategory <= 0)
                .Should().BeNullOrEmpty();
            paymentsCatResultPage_2?
                .Where(pcr => string.IsNullOrEmpty(pcr.Name) ||
                    string.IsNullOrEmpty(pcr.GeneralCategory.Name))
                .Should().BeNullOrEmpty();

            notFoundRequest_1.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_2?.Contains("is invalid").Should().BeTrue();

            badRequest_3.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_3?.Contains("is invalid").Should().BeTrue();
        }

        [Fact]
        public async void UserController_GetPaymentCategoryAsync_ReturnsPaymentCategoryOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var userApiService = new UserApiService(await dbContext);
            var userController = new UserController(_config, userApiService, _mapper);
            var user_1 = new { IdUser = 1, IdDetailedCategories = new List<int>() { 1, 3, 4, 5, 8, 9 } };
            var user_2 = new { IdUser = 2, IdDetailedCategories = new List<int>() { 2, 6, 10, 12 } };
            var user_3 = new { IdUser = 3, IdDetailedCategories = new List<int>() { 7, 11, 13 } };
            var user_4 = new { IdUser = 4, IdDetailedCategories = new List<int>() { 7, -11 } };
            var userCategories_1 = new List<GetUserCategoryDto?>();
            var userCategories_2 = new List<GetUserCategoryDto?>();
            var userCategories_3 = new List<GetUserCategoryDto?>();

            //Act
            foreach (var idDetailedCategory in user_1.IdDetailedCategories)
            {
                var okRequest = await userController.GetPaymentCategoryAsync(user_1.IdUser, idDetailedCategory);
                var okActionResult = okRequest as OkObjectResult;
                userCategories_1.Add(okActionResult?.Value as GetUserCategoryDto);
            }

            foreach (var idDetailedCategory in user_2.IdDetailedCategories)
            {
                var okRequest = await userController.GetPaymentCategoryAsync(user_2.IdUser, idDetailedCategory);
                var okActionResult = okRequest as OkObjectResult;
                userCategories_2.Add(okActionResult?.Value as GetUserCategoryDto);
            }

            foreach (var idDetailedCategory in user_3.IdDetailedCategories)
            {
                var okRequest = await userController.GetPaymentCategoryAsync(user_3.IdUser, idDetailedCategory);
                var okActionResult = okRequest as OkObjectResult;
                userCategories_3.Add(okActionResult?.Value as GetUserCategoryDto);
            }

            var notfoundRequest_1 =
                await userController.GetPaymentCategoryAsync(user_4.IdUser, user_4.IdDetailedCategories.First());

            var badRequest_1 =
                await userController.GetPaymentCategoryAsync(-user_4.IdUser, user_4.IdDetailedCategories.First());
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

            var badRequest_2 =
                await userController.GetPaymentCategoryAsync(user_4.IdUser, user_4.IdDetailedCategories.Last());
            var badRequestActionResult_2 = badRequest_2 as BadRequestObjectResult;
            var badRequestResult_2 = badRequestActionResult_2?.Value as string;

            //Assert
            userCategories_1.Should().NotBeNullOrEmpty();
            userCategories_1?.Count().Should().Be(6);
            userCategories_1?
                .Where(pcr => pcr?.IdDetailedCategory <= 0 ||
                pcr?.GeneralCategory.IdGeneralCategory <= 0)
                .Should().BeNullOrEmpty();
            userCategories_1?
                .Where(pcr => string.IsNullOrEmpty(pcr?.Name) ||
                string.IsNullOrEmpty(pcr.GeneralCategory.Name))
                .Should().BeNullOrEmpty();

            userCategories_2.Should().NotBeNullOrEmpty();
            userCategories_2?.Count().Should().Be(4);
            userCategories_2?
                .Where(pcr => pcr?.IdDetailedCategory <= 0 ||
                pcr?.GeneralCategory.IdGeneralCategory <= 0)
                .Should().BeNullOrEmpty();
            userCategories_2?
                .Where(pcr => string.IsNullOrEmpty(pcr?.Name) ||
                string.IsNullOrEmpty(pcr.GeneralCategory.Name))
                .Should().BeNullOrEmpty();

            userCategories_3.Should().NotBeNullOrEmpty();
            userCategories_3?.Count().Should().Be(3);
            userCategories_3?
                .Where(pcr => pcr?.IdDetailedCategory <= 0 ||
                pcr?.GeneralCategory.IdGeneralCategory <= 0)
                .Should().BeNullOrEmpty();
            userCategories_3?
                .Where(pcr => string.IsNullOrEmpty(pcr?.Name) ||
                string.IsNullOrEmpty(pcr.GeneralCategory.Name))
                .Should().BeNullOrEmpty();

            notfoundRequest_1.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_1?.Value.Should().NotBeNull();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_2?.Value.Should().NotBeNull();
            badRequestResult_2?.Contains("is invalid").Should().BeTrue();
        }
    }
}
