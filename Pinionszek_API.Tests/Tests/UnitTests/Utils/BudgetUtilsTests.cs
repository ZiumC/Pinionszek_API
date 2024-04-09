using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Services.DatabaseServices.BudgetService;
using Pinionszek_API.Services.DatabaseServices.PaymentService;
using Pinionszek_API.Tests.DbContexts;
using Pinionszek_API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinionszek_API.Tests.Tests.UnitTests.Utils
{
    public class BudgetUtilsTests
    {
        private readonly IConfiguration _config;

        public BudgetUtilsTests()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                     path: "appsettings.json",
                     optional: false,
                     reloadOnChange: true)
               .Build();
        }

        [Fact]
        public async Task BudgetUtils_GetPaymentsSum_ReturnsValuesOrThrowsExteption()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var paymentApiService = new PaymentApiService(await dbContext);
            var budgetUtils = new BudgetUtils(_config);
            int idBudget_1 = 1;
            int idBudget_2 = 2;
            int idBudget_13 = 13;

            //Act
            var budgetPaymentsIdBudget_1 =
               await paymentApiService.GetPaymentsAsync(idBudget_1);

            var budgetPaymentsIdBudget_2 =
                await paymentApiService.GetPaymentsAsync(idBudget_2);

            var budgetPaymentsIdBudget_13 =
                await paymentApiService.GetPaymentsAsync(idBudget_13);

            //Assert
            budgetUtils
                .GetPaymentsSum(GeneralCatEnum.NEEDS, PaymentColEnum.CHARGE, budgetPaymentsIdBudget_1)
                .Should().Be(new decimal(1275.99));
            budgetUtils
                .GetPaymentsSum(GeneralCatEnum.WANTS, PaymentColEnum.CHARGE, budgetPaymentsIdBudget_1)
                .Should().Be(new decimal(244.35));
            budgetUtils
                .GetPaymentsSum(GeneralCatEnum.SAVINGS, PaymentColEnum.CHARGE, budgetPaymentsIdBudget_1)
                .Should().Be(new decimal(100));
            budgetUtils
                .GetPaymentsSum(GeneralCatEnum.NO_CATEGORY, PaymentColEnum.REFOUND, budgetPaymentsIdBudget_1)
                .Should().Be(new decimal(86.00));

            budgetUtils
                .GetPaymentsSum(GeneralCatEnum.NEEDS, PaymentColEnum.CHARGE, budgetPaymentsIdBudget_2)
                .Should().Be(new decimal(0.00));
            budgetUtils
                .GetPaymentsSum(GeneralCatEnum.WANTS, PaymentColEnum.CHARGE, budgetPaymentsIdBudget_2)
                .Should().Be(new decimal(0.00));
            budgetUtils
                .GetPaymentsSum(GeneralCatEnum.SAVINGS, PaymentColEnum.CHARGE, budgetPaymentsIdBudget_2)
                .Should().Be(new decimal(0.00));
            budgetUtils
                .GetPaymentsSum(GeneralCatEnum.NO_CATEGORY, PaymentColEnum.REFOUND, budgetPaymentsIdBudget_2)
                .Should().Be(new decimal(0.00));

            budgetUtils
                .GetPaymentsSum(GeneralCatEnum.NEEDS, PaymentColEnum.CHARGE, budgetPaymentsIdBudget_13)
                .Should().Be(new decimal(1166.00));
            budgetUtils
                .GetPaymentsSum(GeneralCatEnum.WANTS, PaymentColEnum.CHARGE, budgetPaymentsIdBudget_13)
                .Should().Be(new decimal(532.01));
            budgetUtils
                .GetPaymentsSum(GeneralCatEnum.SAVINGS, PaymentColEnum.CHARGE, budgetPaymentsIdBudget_13)
                .Should().Be(new decimal(200));
            budgetUtils
                .GetPaymentsSum(GeneralCatEnum.NO_CATEGORY, PaymentColEnum.REFOUND, budgetPaymentsIdBudget_13)
                .Should().Be(new decimal(40.00));
        }
    }
}
