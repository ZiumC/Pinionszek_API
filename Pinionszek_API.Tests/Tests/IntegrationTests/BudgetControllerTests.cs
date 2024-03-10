using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pinionszek_API.Controllers;
using Pinionszek_API.Models.DTOs.GetDTO;
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
        public async Task BudgetController_GetUpcomingPrivatePaymentsAsync_ReturnsOk()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);
            var budgetController = new BudgetController(budgetApiService, _mapper);

            //Act
            var ok_request_1 = await budgetController.GetUpcomingPrivatePaymentsAsync(1, DateTime.Parse("2024-01-01"));
            var ok_request_2 = await budgetController.GetUpcomingPrivatePaymentsAsync(2, DateTime.Parse("2024-01-01"));
            var notfound_request_1 = await budgetController.GetUpcomingPrivatePaymentsAsync(3, DateTime.Parse("2024-01-01"));
            var notfound_request_2 = await budgetController.GetUpcomingPrivatePaymentsAsync(4, DateTime.Parse("2024-01-01"));
            var badrequest_request_1 = await budgetController.GetUpcomingPrivatePaymentsAsync(-10, DateTime.Parse("2024-01-01"));

            //Assert
            ok_request_1.Should().BeOfType<OkObjectResult>();
            ok_request_2.Should().BeOfType<OkObjectResult>();
            notfound_request_1.Should().BeOfType<NotFoundResult>();
            notfound_request_2.Should().BeOfType<NotFoundResult>();
            badrequest_request_1.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
