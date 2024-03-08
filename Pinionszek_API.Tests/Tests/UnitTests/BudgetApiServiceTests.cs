using AutoMapper;
using FluentAssertions;
using Pinionszek_API.Models.DTOs.GetDTO;
using Pinionszek_API.Profiles;
using Pinionszek_API.Services.DatabaseServices.BudgetService;
using Pinionszek_API.Tests.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinionszek_API.Tests.Tests.UnitTests
{
    public class BudgetApiServiceTests
    {
        private readonly IMapper _mapper;
        public BudgetApiServiceTests()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PaymentProfile());
                cfg.AddProfile(new CategoryProfile());
            });
            _mapper = mockMapper.CreateMapper();
        }
        [Fact]
        public async Task BudgetApiService_GetBudgetAsync_ReturnsBudgets()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);

            //Act
            var budget_of_idUser_1 = await budgetApiService.GetBudgetAsync(1, DateTime.Parse("2024-01-01"));
            var payments_of_idUser_1 =
                _mapper.Map<List<PrivatePaymentDTO>>(budget_of_idUser_1?.Payments);

            var budget_of_idUser_2 = await budgetApiService.GetBudgetAsync(2, DateTime.Parse("2024-01-01"));
            var payments_of_idUser_2 =
                _mapper.Map<IEnumerable<PrivatePaymentDTO>>(
                    _mapper.Map<List<PrivatePaymentDTO>>(budget_of_idUser_2?.Payments)
                );

            var budget_of_idUser_3 = await budgetApiService.GetBudgetAsync(3, DateTime.Parse("2024-01-01"));
            var payments_of_idUser_3 =
                _mapper.Map<IEnumerable<PrivatePaymentDTO>>(
                    _mapper.Map<List<PrivatePaymentDTO>>(budget_of_idUser_3?.Payments)
                );

            var budget_of_idUser_4 = await budgetApiService.GetBudgetAsync(4, DateTime.Parse("2024-01-01"));
            var payments_of_idUser_4 =
                _mapper.Map<IEnumerable<PrivatePaymentDTO>>(
                    _mapper.Map<List<PrivatePaymentDTO>>(budget_of_idUser_4?.Payments)
                );


            //Assert
            payments_of_idUser_1.Should().NotBeNull();
            payments_of_idUser_1.Count().Should().Be(7);
            payments_of_idUser_1
                .Where(
                    p => p.Category == null ||
                    string.IsNullOrEmpty(p.Category.DetailedName) ||
                    string.IsNullOrEmpty(p.Category.GeneralName)
                )
                .ToList().Count().Should().Be(0);
            payments_of_idUser_1
                .Where(
                    p => p.IdSharedPayment > 0
                )
                .ToList().Count().Should().Be(2);
            payments_of_idUser_1
                .Where(
                    p => string.IsNullOrEmpty(p.Status)
                )
                .ToList().Count().Should().Be(0);

            payments_of_idUser_2.Should().NotBeNull();
            payments_of_idUser_2.Count().Should().Be(6);
            payments_of_idUser_2
                .Where(
                p => p.Category == null ||
                string.IsNullOrEmpty(p.Category.DetailedName) ||
                string.IsNullOrEmpty(p.Category.GeneralName)
                )
                .ToList().Count().Should().Be(0);
            payments_of_idUser_2
                .Where(
                p => p.IdSharedPayment > 0
                )
                .ToList().Count().Should().Be(1);
            payments_of_idUser_2
                .Where(
                    p => string.IsNullOrEmpty(p.Status)
                )
                .ToList().Count().Should().Be(0);

            payments_of_idUser_3.Should().NotBeNull();
            payments_of_idUser_3.Count().Should().Be(0);

            payments_of_idUser_3.Should().NotBeNull();
            payments_of_idUser_3.Count().Should().Be(0);
        }

        [Fact]
        public async Task BudgetApiService_GetFriendNameAsync_ReturnsString()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var budgetApiService = new BudgetApiService(await dbContext);

            //Act
            var result_of_idSharedPayment_1 = await budgetApiService.GetFriendNameAsync(1);
            var result_of_idSharedPayment_2 = await budgetApiService.GetFriendNameAsync(2);
            var result_of_idSharedPayment_3 = await budgetApiService.GetFriendNameAsync(3);
            var result_empty = await budgetApiService.GetFriendNameAsync(1001);

            //Assert
            result_of_idSharedPayment_1.Should().NotBeNull();
            result_of_idSharedPayment_1.Should().BeOfType<string>();

            result_of_idSharedPayment_2.Should().NotBeNull();
            result_of_idSharedPayment_2.Should().BeOfType<string>();

            result_of_idSharedPayment_3.Should().NotBeNull();
            result_of_idSharedPayment_3.Should().BeOfType<string>();

            result_empty.Should().BeNull();
        }
    }
}
