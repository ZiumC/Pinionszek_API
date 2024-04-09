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
    public class BudgetControllerTests
    {

        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public BudgetControllerTests()
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
        }

        [Fact]
        public async Task BudgetController_GetBudgetSummaryAsync_ReturnsPaymentsOrNotfoundOrBadrequest()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var paymentApiService = new PaymentApiService(await dbContext);
            var budgetController = new BudgetsController(_config, budgetApiService, paymentApiService, _mapper);
            var budgetDate = DateTime.Parse("2024-01-01");

            //Act
            var okRequest_1 = await budgetController.GetBudgetSummaryAsync(budgetDate, 1);
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var budgetResult_1 = okActionResult_1?.Value as GetBudgetSummaryDto;

            var okRequest_2 = await budgetController.GetBudgetSummaryAsync(budgetDate, 2);
            var okActionResult_2 = okRequest_2 as OkObjectResult;
            var budgetResult_2 = okActionResult_2?.Value as GetBudgetSummaryDto;

            var okRequest_3 = await budgetController.GetBudgetSummaryAsync(budgetDate, 3);
            var okActionResult_3 = okRequest_3 as OkObjectResult;
            var budgetResult_3 = okActionResult_3?.Value as GetBudgetSummaryDto;

            var notFoundRequest_1 = await budgetController.GetBudgetSummaryAsync(budgetDate, 1001);

            var badRequest_1 = await budgetController.GetBudgetSummaryAsync(budgetDate, -1005);
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

            var badRequest_2 = await budgetController.GetBudgetSummaryAsync(new DateTime(), 1005);
            var badRequestActionResult_2 = badRequest_2 as BadRequestObjectResult;
            var badRequestResult_2 = badRequestActionResult_2?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            okActionResult_1.Should().NotBeNull();
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
            okActionResult_2.Should().NotBeNull();
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
            okActionResult_3.Should().NotBeNull();
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
            badRequestActionResult_1?.Value.Should().NotBeNull();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_2?.Value.Should().NotBeNull();
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
            int budget_year = 2024;

            //Act
            var okRequest_1 = await budgetController.GetBudgetsAsync(budget_year, 1);
            var okActionResult_1 = okRequest_1 as OkObjectResult;
            var budgetsResult_1 = okActionResult_1?.Value as IEnumerable<GetBudgetSummaryDto>;

            var okRequest_2 = await budgetController.GetBudgetsAsync(budget_year, 2);
            var okActionResult_2 = okRequest_2 as OkObjectResult;
            var budgetsResult_2 = okActionResult_2?.Value as IEnumerable<GetBudgetSummaryDto>;

            var okRequest_3 = await budgetController.GetBudgetsAsync(budget_year, 3);
            var okActionResult_3 = okRequest_3 as OkObjectResult;
            var budgetsResult_3 = okActionResult_3?.Value as IEnumerable<GetBudgetSummaryDto>;

            var notFoundRequest_1 = await budgetController.GetBudgetsAsync(budget_year, 4);

            var badRequest_1 = await budgetController.GetBudgetsAsync(budget_year, -1005);
            var badRequestActionResult_1 = badRequest_1 as BadRequestObjectResult;
            var badRequestResult_1 = badRequestActionResult_1?.Value as string;

            var badRequest_2 = await budgetController.GetBudgetsAsync(-1, 1);
            var badRequestActionResult_2 = badRequest_2 as BadRequestObjectResult;
            var badRequestResult_2 = badRequestActionResult_2?.Value as string;

            var badRequest_3 = await budgetController.GetBudgetsAsync(99999, 1);
            var badRequestActionResult_3 = badRequest_3 as BadRequestObjectResult;
            var badRequestResult_3 = badRequestActionResult_3?.Value as string;

            //Assert
            okRequest_1.Should().BeOfType<OkObjectResult>();
            okActionResult_1.Should().NotBeNull();
            budgetsResult_1.Should().NotBeNull();
            budgetsResult_1?.Count().Should().Be(12);
            budgetsResult_1?
                .Where(br => string.IsNullOrEmpty(br.Budget.Status))
                .ToList().Should().BeNullOrEmpty();
            budgetsResult_1?
                .Where(br => br.Budget.BudgetYear.Year != budget_year)
                .ToList().Should().BeNullOrEmpty();
            budgetsResult_1?
                .Where(br => br.Needs < 0 || br.Wants < 0 || br.Savings < 0)
                .ToList().Should().BeNullOrEmpty();

            okRequest_2.Should().BeOfType<OkObjectResult>();
            okActionResult_2.Should().NotBeNull();
            budgetsResult_2.Should().NotBeNull();
            budgetsResult_2?.Count().Should().Be(12);
            budgetsResult_2?
                .Where(br => string.IsNullOrEmpty(br.Budget.Status))
                .ToList().Should().BeNullOrEmpty();
            budgetsResult_2?
                .Where(br => br.Budget.BudgetYear.Year != budget_year)
                .ToList().Should().BeNullOrEmpty();
            budgetsResult_2?
                .Where(br => br.Needs < 0 || br.Wants < 0 || br.Savings < 0)
                .ToList().Should().BeNullOrEmpty();

            okRequest_3.Should().BeOfType<OkObjectResult>();
            okActionResult_3.Should().NotBeNull();
            budgetsResult_3.Should().NotBeNull();
            budgetsResult_3?.Count().Should().Be(12);
            budgetsResult_3?
                .Where(br => string.IsNullOrEmpty(br.Budget.Status))
                .ToList().Should().BeNullOrEmpty();
            budgetsResult_3?
                .Where(br => br.Budget.BudgetYear.Year != budget_year)
                .ToList().Should().BeNullOrEmpty();
            budgetsResult_3?
                .Where(br => br.Needs < 0 || br.Wants < 0 || br.Savings < 0)
                .ToList().Should().BeNullOrEmpty();

            notFoundRequest_1.Should().BeOfType<NotFoundResult>();

            badRequest_1.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_1?.Value.Should().NotBeNull();
            badRequestResult_1?.Contains("is invalid").Should().BeTrue();

            badRequest_2.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_2?.Value.Should().NotBeNull();
            badRequestResult_2?.Contains("is invalid").Should().BeTrue();

            badRequest_3.Should().BeOfType<BadRequestObjectResult>();
            badRequestActionResult_3?.Value.Should().NotBeNull();
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
