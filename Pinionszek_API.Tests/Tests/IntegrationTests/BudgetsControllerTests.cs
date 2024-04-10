using AutoMapper;
using Castle.Components.DictionaryAdapter.Xml;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Moq;
using Newtonsoft.Json;
using Pinionszek_API.Controllers;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Models.DTOs.GetDto;
using Pinionszek_API.Models.DTOs.GetDto.Payments;
using Pinionszek_API.Models.DTOs.GetDto.User;
using Pinionszek_API.Profiles;
using Pinionszek_API.Services.DatabaseServices.BudgetService;
using Pinionszek_API.Services.DatabaseServices.PaymentService;
using Pinionszek_API.Tests.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinionszek_API.Tests.Tests.IntegrationTests
{
    public class BudgetsControllerTests
    {

        private readonly int _user_1 = 1;
        private readonly int _user_2 = 2;
        private readonly int _user_3 = 3;
        private readonly int _user_4 = 4;
        private readonly int _user_5 = 100;
        private readonly int _budget_year = 2024;
        private readonly DateTime _budgetDate;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public BudgetsControllerTests()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PaymentProfile());
                cfg.AddProfile(new CategoryProfile());
                cfg.AddProfile(new BudgetProfile());
            });
            _mapper = mockMapper.CreateMapper();

            _config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(
                 path: "appsettings.json",
                 optional: false,
                 reloadOnChange: true)
           .Build();

            _budgetDate = DateTime.Parse("2024-01-01");
        }

        [Fact]
        public async Task BudgetController_GetBudgetSummaryAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var paymentApiService = new PaymentApiService(await dbContext);
            var budgetController = new BudgetsController(_config, budgetApiService, paymentApiService, _mapper);


            //Act
            var okRequest_1 = await budgetController.GetBudgetSummaryAsync(_budgetDate, _user_1);
            var budgetResult_1 = (okRequest_1 as OkObjectResult)?.Value as GetBudgetSummaryDto;

            var okRequest_2 = await budgetController.GetBudgetSummaryAsync(_budgetDate, _user_2);
            var budgetResult_2 = (okRequest_2 as OkObjectResult)?.Value as GetBudgetSummaryDto;

            var okRequest_3 = await budgetController.GetBudgetSummaryAsync(_budgetDate, _user_3);
            var budgetResult_3 = (okRequest_3 as OkObjectResult)?.Value as GetBudgetSummaryDto;

            var notFoundRequest_1 = await budgetController.GetBudgetSummaryAsync(_budgetDate, _user_5);

            var badRequest_1 = await budgetController.GetBudgetSummaryAsync(_budgetDate, -_user_5);
            var badRequestResult_1 = (badRequest_1 as BadRequestObjectResult)?.Value as string;

            var badRequest_2 = await budgetController.GetBudgetSummaryAsync(new DateTime(), _user_5);
            var badRequestResult_2 = (badRequest_2 as BadRequestObjectResult)?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            budgetResult_1.Should().NotBeNull();
            budgetResult_1?.Budget.Should().NotBeNull();
            budgetResult_1?.Budget.Status.Should().NotBeNullOrEmpty();
            budgetResult_1?.Budget.BudgetYear.Should()
                .NotBeBefore(DateTime.Parse("2024-01-01")).And
                .NotBeAfter(DateTime.Parse("2024-01-31"));
            budgetResult_1?.Budget.IsCompleted.Should().BeFalse();
            budgetResult_1?.Needs.Should().BeGreaterThanOrEqualTo(0);
            budgetResult_1?.Wants.Should().BeGreaterThanOrEqualTo(0);
            budgetResult_1?.Savings.Should().BeGreaterThanOrEqualTo(0);


            okRequest_2.Should().BeOfType<OkObjectResult>();
            budgetResult_2.Should().NotBeNull();
            budgetResult_2?.Budget.Should().NotBeNull();
            budgetResult_2?.Budget.Status.Should().NotBeNullOrEmpty();
            budgetResult_2?.Budget.BudgetYear.Should()
                .NotBeBefore(DateTime.Parse("2024-01-01")).And
                .NotBeAfter(DateTime.Parse("2024-01-31"));
            budgetResult_2?.Budget.IsCompleted.Should().BeFalse();
            budgetResult_2?.Needs.Should().BeGreaterThanOrEqualTo(0);
            budgetResult_2?.Wants.Should().BeGreaterThanOrEqualTo(0);
            budgetResult_2?.Savings.Should().BeGreaterThanOrEqualTo(0);

            okRequest_3.Should().BeOfType<OkObjectResult>();
            budgetResult_3.Should().NotBeNull();
            budgetResult_3?.Budget.Should().NotBeNull();
            budgetResult_3?.Budget.Status.Should().NotBeNullOrEmpty();
            budgetResult_3?.Budget.BudgetYear.Should()
                .NotBeBefore(DateTime.Parse("2024-01-01")).And
                .NotBeAfter(DateTime.Parse("2024-01-31"));
            budgetResult_3?.Budget.IsCompleted.Should().BeFalse();
            budgetResult_3?.Needs.Should().BeGreaterThanOrEqualTo(0);
            budgetResult_3?.Wants.Should().BeGreaterThanOrEqualTo(0);
            budgetResult_3?.Savings.Should().BeGreaterThanOrEqualTo(0);

            notFoundRequest_1.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_2?.Contains("is not specified").Should().BeTrue();
        }

        [Fact]
        public async Task BudgetController_GetBudgetsAsync_ReturnsBudgetsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var paymentApiService = new PaymentApiService(await dbContext);
            var budgetController = new BudgetsController(_config, budgetApiService, paymentApiService, _mapper);
            var budgetYears = new { invalid1 = -1, invalid2 = 99999 };

            //Act
            var okRequest_1 = await budgetController.GetBudgetsAsync(_budget_year, _user_1);
            var budgetsResult_1 = (okRequest_1 as OkObjectResult)?.Value as IEnumerable<GetBudgetSummaryDto>;

            var okRequest_2 = await budgetController.GetBudgetsAsync(_budget_year, _user_2);
            var budgetsResult_2 = (okRequest_2 as OkObjectResult)?.Value as IEnumerable<GetBudgetSummaryDto>;

            var okRequest_3 = await budgetController.GetBudgetsAsync(_budget_year, _user_3);
            var budgetsResult_3 = (okRequest_3 as OkObjectResult)?.Value as IEnumerable<GetBudgetSummaryDto>;

            var notFoundRequest_1 = await budgetController.GetBudgetsAsync(_budget_year, _user_4);

            var badRequest_1 = await budgetController.GetBudgetsAsync(_budget_year, -_user_5);
            var badRequestResult_1 = (badRequest_1 as BadRequestObjectResult)?.Value as string;

            var badRequest_2 = await budgetController.GetBudgetsAsync(budgetYears.invalid1, _user_1);
            var badRequestResult_2 = (badRequest_2 as BadRequestObjectResult)?.Value as string;

            var badRequest_3 = await budgetController.GetBudgetsAsync(budgetYears.invalid2, _user_1);
            var badRequestResult_3 = (badRequest_3 as BadRequestObjectResult)?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            budgetsResult_1.Should().NotBeNull();
            budgetsResult_1?.Count().Should().Be(12);
            budgetsResult_1?
                .Where(br => string.IsNullOrEmpty(br.Budget.Status))
                .ToList().Should().BeNullOrEmpty();
            budgetsResult_1?
                .Where(br => br.Budget.BudgetYear.Year != _budget_year)
                .ToList().Should().BeNullOrEmpty();
            budgetsResult_1?
                .Where(br => br.Needs < 0 || br.Wants < 0 || br.Savings < 0)
                .ToList().Should().BeNullOrEmpty();

            okRequest_2.Should().BeOfType<OkObjectResult>();
            budgetsResult_2.Should().NotBeNull();
            budgetsResult_2?.Count().Should().Be(12);
            budgetsResult_2?
                .Where(br => string.IsNullOrEmpty(br.Budget.Status))
                .ToList().Should().BeNullOrEmpty();
            budgetsResult_2?
                .Where(br => br.Budget.BudgetYear.Year != _budget_year)
                .ToList().Should().BeNullOrEmpty();
            budgetsResult_2?
                .Where(br => br.Needs < 0 || br.Wants < 0 || br.Savings < 0)
                .ToList().Should().BeNullOrEmpty();

            okRequest_3.Should().BeOfType<OkObjectResult>();
            budgetsResult_3.Should().NotBeNull();
            budgetsResult_3?.Count().Should().Be(12);
            budgetsResult_3?
                .Where(br => string.IsNullOrEmpty(br.Budget.Status))
                .ToList().Should().BeNullOrEmpty();
            budgetsResult_3?
                .Where(br => br.Budget.BudgetYear.Year != _budget_year)
                .ToList().Should().BeNullOrEmpty();
            budgetsResult_3?
                .Where(br => br.Needs < 0 || br.Wants < 0 || br.Savings < 0)
                .ToList().Should().BeNullOrEmpty();

            notFoundRequest_1.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_2?.Contains("is invalid").Should().BeTrue();

            badRequest_3.Should().BeOfType<BadRequestObjectResult>();
            badRequestResult_3?.Contains("is invalid").Should().BeTrue();
        }

        [Fact]
        public async Task BudgetController_GetDefaultGeneralCategoriesAsync_ReturnsCategoriesOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var paymentApiService = new PaymentApiService(await dbContext);
            var budgetController = new BudgetsController(_config, budgetApiService, paymentApiService, _mapper);

            //Act
            var okRequest_1 = await budgetController.GetDefaultGeneralCategoriesAsync();
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var generalCategoriessResult_1 = okActionResult_1?.Value as IEnumerable<GetGeneralCategoryDto>;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            okActionResult_1.Should().NotBeNull();
            generalCategoriessResult_1.Should().NotBeNullOrEmpty();
            generalCategoriessResult_1?.Count().Should().Be(3);
            generalCategoriessResult_1?
                .Where(gcr => string.IsNullOrEmpty(gcr.Name))
                .Should().BeNullOrEmpty();
            generalCategoriessResult_1?
                .Where(gcr => gcr.IdGeneralCategory <= 0)
                .Should().BeNullOrEmpty();
            generalCategoriessResult_1?
                .Where(gcr => gcr.IsDefault)
                .Count().Should().Be(3);
        }
    }
}
