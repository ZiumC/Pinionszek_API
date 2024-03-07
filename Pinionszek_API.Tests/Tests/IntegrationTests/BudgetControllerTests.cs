using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Pinionszek_API.Controllers;
using Pinionszek_API.Services.DatabaseServices.BudgetService;
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
        public BudgetControllerTests()
        {
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public async Task BudgetController_GetUpcomingPaymentsAsync_ReturnsOk()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var budgetController = new BudgetController(budgetApiService, _mapper);

            //Act
            var result1 = await budgetController.GetUpcomingPaymentsAsync(1, DateTime.Parse("2024-01-01"));
            var result2 = await budgetController.GetUpcomingPaymentsAsync(2, DateTime.Parse("2024-01-01"));
            var result3 = await budgetController.GetUpcomingPaymentsAsync(3, DateTime.Parse("2024-01-01"));
            var result4 = await budgetController.GetUpcomingPaymentsAsync(4, DateTime.Parse("2024-01-01"));

            //Assert
            result1.Should().NotBeNull();
            result1.Should().BeOfType<OkObjectResult>();

            result2.Should().NotBeNull();
            result2.Should().BeOfType<OkObjectResult>();


            result3.Should().NotBeNull();
            result3.Should().BeOfType<NotFoundResult>();

            result4.Should().NotBeNull();
            result4.Should().BeOfType<NotFoundResult>();

            result1.Equals(result2).Should().Be(false);

        }
    }
}
