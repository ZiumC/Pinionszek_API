using FluentAssertions;
using Pinionszek_API.Services.DatabaseServices.PaymentService;
using Pinionszek_API.Tests.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinionszek_API.Tests.Tests.UnitTests.ApiServices
{
    public class PaymentsApiServiceTests
    {
        [Fact]
        public async Task BudgetApiService_GetPaymentsAsync_ReturnsPaymentsOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var paymentApiService = new PaymentApiService(await dbContext);

            //Act
            var paymentsOfIdUser_1 = await paymentApiService.GetPaymentsAsync(1);
            var paymentsOfIdUser_2 = await paymentApiService.GetPaymentsAsync(13);
            var paymentsOfNonExistingBudget = await paymentApiService.GetPaymentsAsync(37);

            //Assert
            paymentsOfIdUser_1.Should().NotBeEmpty();
            paymentsOfIdUser_1.Count().Should().Be(7);
            paymentsOfIdUser_1
                .Where(p => p.DetailedCategory == null ||
                    string.IsNullOrEmpty(p.DetailedCategory.Name) ||
                    p.DetailedCategory.IdGeneralCategory == 0 ||
                    p.DetailedCategory.GeneralCategory.IdGeneralCategory == 0 ||
                    string.IsNullOrEmpty(p.DetailedCategory?.GeneralCategory.Name))
                .ToList().Count()
                .Should().Be(0);
            paymentsOfIdUser_1
                .Where(p => p.IdPaymentStatus == 0)
                .ToList().Count()
                .Should().Be(0);
            paymentsOfIdUser_1
                .Where(p => string.IsNullOrEmpty(p.Name))
                .ToList().Count().Should().Be(0);

            paymentsOfIdUser_2.Should().NotBeEmpty();
            paymentsOfIdUser_2.Count().Should().Be(6);
            paymentsOfIdUser_2
                .Where(p => p.DetailedCategory == null ||
                    string.IsNullOrEmpty(p.DetailedCategory.Name) ||
                    p.DetailedCategory.IdGeneralCategory == 0 ||
                    p.DetailedCategory.GeneralCategory.IdGeneralCategory == 0 ||
                    string.IsNullOrEmpty(p.DetailedCategory?.GeneralCategory.Name))
                .ToList().Count()
                .Should().Be(0);
            paymentsOfIdUser_2
                .Where(p => p.IdPaymentStatus == 0)
                .ToList().Count()
                .Should().Be(0);
            paymentsOfIdUser_2
                .Where(p => string.IsNullOrEmpty(p.Name))
                .ToList().Count().Should().Be(0);


            paymentsOfNonExistingBudget.Should().BeEmpty();
        }
    }
}
