using FluentAssertions;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Services.DatabaseServices.BudgetService;
using Pinionszek_API.Tests.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinionszek_API.Tests.Controller
{
    public class BudgetController
    {

        public BudgetController()
        {
        }

        [Fact]
        public async void BudgetController_GetUpcomingPrivatePaymentsAsync_ReturnsPayments()
        {
            //Arrange
            var dbContext = new InMemContext();
            var inMemDbContext = await dbContext.GetDatabaseContext();
            var budgetApiService = new BudgetApiService(inMemDbContext);

            //Act
            var result = await budgetApiService.GetBudgetAsync(1, DateTime.Parse("2024-01-01"));

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Budget>();
        }
    }
}
