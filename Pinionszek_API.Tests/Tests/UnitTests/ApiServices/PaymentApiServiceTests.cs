using FluentAssertions;
using Pinionszek_API.Models.DatabaseModel;
using Pinionszek_API.Services.DatabaseServices.BudgetService;
using Pinionszek_API.Services.DatabaseServices.PaymentService;
using Pinionszek_API.Tests.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinionszek_API.Tests.Tests.UnitTests.ApiServices
{
    public class PaymentApiServiceTests
    {
        private readonly int _idBudget_1 = 1;
        private readonly int _idBudget_13 = 13;
        private readonly int _idBudget_37 = 37;
        private readonly int _idSharedPayment_1 = 1;
        private readonly int _idSharedPayment_4 = 4;
        private readonly int _idSharedPayment_10 = 10;
        private readonly int _idSharedPayment_100 = 100;
        private readonly int _idPayment_1 = 1;
        private readonly int _idPayment_2 = 2;
        private readonly int _idPayment_4 = 4;
        private readonly int _idPayment_7 = 7;
        private readonly int _idPayment_13 = 13;
        private readonly int _user_1 = 1;
        private readonly int _user_2 = 2;
        private readonly int _friend_1 = 1001;
        private readonly int _friend_2 = 1002;
        private readonly int _friend_3 = 1003;
        private readonly int _friend_4 = 1004;
        [Fact]
        public async Task PaymentsApiService_GetPaymentsAsync_ReturnsPaymentsOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var paymentApiService = new PaymentApiService(await dbContext);

            //Act
            var paymentsResult_1 = await paymentApiService.GetPaymentsAsync(_idBudget_1);
            var paymentsResult_2 = await paymentApiService.GetPaymentsAsync(_idBudget_13);
            var paymentsResult_3 = await paymentApiService.GetPaymentsAsync(_idBudget_37);

            //Assert
            paymentsResult_1.Should().NotBeEmpty();
            paymentsResult_1.Count().Should().Be(7);
            paymentsResult_1
                .Where(p => p.DetailedCategory == null ||
                    string.IsNullOrEmpty(p.DetailedCategory.Name) ||
                    p.DetailedCategory.IdGeneralCategory == 0 ||
                    p.DetailedCategory.GeneralCategory.IdGeneralCategory == 0 ||
                    string.IsNullOrEmpty(p.DetailedCategory?.GeneralCategory.Name))
                .ToList().Count().Should().Be(0);
            paymentsResult_1
                .Where(p => p.IdPaymentStatus == 0)
                .ToList().Count().Should().Be(0);
            paymentsResult_1
                .Where(p => string.IsNullOrEmpty(p.Name))
                .ToList().Count().Should().Be(0);

            paymentsResult_2.Should().NotBeEmpty();
            paymentsResult_2.Count().Should().Be(6);
            paymentsResult_2
                .Where(p => p.DetailedCategory == null ||
                    string.IsNullOrEmpty(p.DetailedCategory.Name) ||
                    p.DetailedCategory.IdGeneralCategory == 0 ||
                    p.DetailedCategory.GeneralCategory.IdGeneralCategory == 0 ||
                    string.IsNullOrEmpty(p.DetailedCategory?.GeneralCategory.Name))
                .ToList().Count().Should().Be(0);
            paymentsResult_2
                .Where(p => p.IdPaymentStatus == 0)
                .ToList().Count().Should().Be(0);
            paymentsResult_2
                .Where(p => string.IsNullOrEmpty(p.Name))
                .ToList().Count().Should().Be(0);

            paymentsResult_3.Should().BeEmpty();
        }

        [Fact]
        public async Task PaymentsApiService_GetSharedPaymentDataAsync_ReturnsSharedPaymentOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var paymentApiService = new PaymentApiService(await dbContext);

            //Act
            var sharedPaymentResult_1 = await paymentApiService.GetSharedPaymentDataAsync
                (_idSharedPayment_1);
            var sharedPaymentResult_2 = await paymentApiService.GetSharedPaymentDataAsync
                (_idSharedPayment_4);
            var sharedPaymentResult_3 = await paymentApiService.GetSharedPaymentDataAsync
                (_idSharedPayment_10);
            var sharedPaymentResult_4 = await paymentApiService.GetSharedPaymentDataAsync
                (_idSharedPayment_100);

            //Assert
            sharedPaymentResult_1.Should().NotBeNull();
            sharedPaymentResult_1?.IdPayment.Should().Be(1);
            sharedPaymentResult_1?.IdFriend.Should().Be(1);

            sharedPaymentResult_2.Should().NotBeNull();
            sharedPaymentResult_2?.IdPayment.Should().Be(4);
            sharedPaymentResult_2?.IdFriend.Should().Be(1);

            sharedPaymentResult_3.Should().NotBeNull();
            sharedPaymentResult_3?.IdPayment.Should().Be(10);
            sharedPaymentResult_3?.IdFriend.Should().Be(3);

            sharedPaymentResult_4.Should().BeNull();
        }

        [Fact]
        public async Task PaymentsApiService_GetAssignedPaymentsAsync_ReturnsPaymentsOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var paymentApiService = new PaymentApiService(await dbContext);

            //Act
            var friendsPaymentResult_1 = await paymentApiService.GetAssignedPaymentsAsync(_friend_1);
            var friendsPaymentResult_2 = await paymentApiService.GetAssignedPaymentsAsync(_friend_2);
            var friendsPaymentResult_3 = await paymentApiService.GetAssignedPaymentsAsync(_friend_3);
            var friendsPaymentResult_4 = await paymentApiService.GetAssignedPaymentsAsync(_friend_4);

            //Assert
            friendsPaymentResult_1.Should().NotBeNullOrEmpty();
            friendsPaymentResult_1.Should().BeOfType<List<Payment>>();
            friendsPaymentResult_1.Count().Should().Be(1);
            friendsPaymentResult_1
                .Where(ap => ap.SharedPayment == null)
                .ToList().Count().Should().Be(0);
            friendsPaymentResult_1
                .Where(ap => ap.DetailedCategory == null ||
                    string.IsNullOrEmpty(ap.DetailedCategory.Name) ||
                    ap.DetailedCategory.IdGeneralCategory == 0 ||
                    ap.DetailedCategory.GeneralCategory.IdGeneralCategory == 0 ||
                    string.IsNullOrEmpty(ap.DetailedCategory?.GeneralCategory.Name))
                .ToList().Count().Should().Be(0);
            friendsPaymentResult_1
                .Where(ap => ap.IdPaymentStatus == 0)
                .ToList().Count()
                .Should().Be(0);
            friendsPaymentResult_1
                .Where(ap => string.IsNullOrEmpty(ap.Name))
                .ToList().Count().Should().Be(0);

            friendsPaymentResult_2.Should().NotBeNullOrEmpty();
            friendsPaymentResult_2.Should().BeOfType<List<Payment>>();
            friendsPaymentResult_2.Count().Should().Be(2);
            friendsPaymentResult_2
                .Where(ap => ap.SharedPayment == null)
                .ToList().Count().Should().Be(0);
            friendsPaymentResult_2
                .Where(ap => ap.DetailedCategory == null ||
                    string.IsNullOrEmpty(ap.DetailedCategory.Name) ||
                    ap.DetailedCategory.IdGeneralCategory == 0 ||
                    ap.DetailedCategory.GeneralCategory.IdGeneralCategory == 0 ||
                    string.IsNullOrEmpty(ap.DetailedCategory?.GeneralCategory.Name))
                .ToList().Count().Should().Be(0);
            friendsPaymentResult_2
                .Where(ap => ap.IdPaymentStatus == 0)
                .ToList().Count()
                .Should().Be(0);
            friendsPaymentResult_2
                .Where(ap => string.IsNullOrEmpty(ap.Name))
                .ToList().Count().Should().Be(0);

            friendsPaymentResult_3.Should().BeNullOrEmpty();
            friendsPaymentResult_4.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task BudgetApiService_GetPaymentAsync_ReturnsPaymentOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var paymentApiService = new PaymentApiService(await dbContext);

            //Act
            var payment1 = await paymentApiService.GetPaymentAsync(_idPayment_1, _user_1);
            var payment2 = await paymentApiService.GetPaymentAsync(_idPayment_7, _user_2);
            var payment3 = await paymentApiService.GetPaymentAsync(_idPayment_2, _user_1);
            var payment4 = await paymentApiService.GetPaymentAsync(_idPayment_4, _user_2);
            var payment5 = await paymentApiService.GetPaymentAsync(_idPayment_13, _user_1);
            var payment6 = await paymentApiService.GetPaymentAsync(_idPayment_4, _user_1);

            //Assert
            payment1.Should().NotBeNull();
            payment1?.Name.Should().NotBeNullOrEmpty();
            payment1?.Charge.Should().BeGreaterThanOrEqualTo(0);
            payment1?.Refund.Should().BeGreaterThanOrEqualTo(0);
            payment1?.PaymentStatus.Should().NotBeNull();
            payment1?.PaymentStatus.Name.Should().NotBeNullOrEmpty();
            payment1?.DetailedCategory.Name.Should().NotBeNullOrEmpty();
            payment1?.DetailedCategory.GeneralCategory.Name.Should().NotBeNullOrEmpty();
            payment1?.SharedPayment.Should().NotBeNull();
            payment1?.SharedPayment?.IdSharedPayment.Should().BeGreaterThan(0);


            payment2.Should().NotBeNull();
            payment2?.Name.Should().NotBeNullOrEmpty();
            payment2?.Charge.Should().BeGreaterThanOrEqualTo(0);
            payment2?.Refund.Should().BeGreaterThanOrEqualTo(0);
            payment2?.PaymentStatus.Should().NotBeNull();
            payment2?.PaymentStatus.Name.Should().NotBeNullOrEmpty();
            payment2?.DetailedCategory.Name.Should().NotBeNullOrEmpty();
            payment2?.DetailedCategory.GeneralCategory.Name.Should().NotBeNullOrEmpty();
            payment2?.SharedPayment.Should().BeNull();

            payment3.Should().NotBeNull();
            payment3?.Name.Should().NotBeNullOrEmpty();
            payment3?.Charge.Should().BeGreaterThanOrEqualTo(0);
            payment3?.Refund.Should().BeGreaterThanOrEqualTo(0);
            payment3?.PaymentStatus.Should().NotBeNull();
            payment3?.PaymentStatus.Name.Should().NotBeNullOrEmpty();
            payment3?.DetailedCategory.Name.Should().NotBeNullOrEmpty();
            payment3?.DetailedCategory.GeneralCategory.Name.Should().NotBeNullOrEmpty();
            payment3?.SharedPayment.Should().BeNull();

            payment4.Should().BeNull();

            payment5.Should().BeNull();

            payment6.Should().NotBeNull();
            payment6?.Name.Should().NotBeNullOrEmpty();
            payment6?.Charge.Should().BeGreaterThanOrEqualTo(0);
            payment6?.Refund.Should().BeGreaterThanOrEqualTo(0);
            payment6?.PaymentStatus.Should().NotBeNull();
            payment6?.PaymentStatus.Name.Should().NotBeNullOrEmpty();
            payment6?.DetailedCategory.Name.Should().NotBeNullOrEmpty();
            payment6?.DetailedCategory.GeneralCategory.Name.Should().NotBeNullOrEmpty();
            payment6?.SharedPayment.Should().NotBeNull();
            payment6?.SharedPayment?.IdSharedPayment.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task PaymentApiService_GetDefaultGeneralCategoriesAsync_ReturnsCategoriesOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var paymentApiService = new PaymentApiService(await dbContext);

            //Act
            var defaultCategories = await paymentApiService.GetDefaultGeneralCategoriesAsync();

            //Assert
            defaultCategories.Should().NotBeNullOrEmpty();
            defaultCategories.Count().Should().Be(3);
            defaultCategories
                .Where(dc => string.IsNullOrEmpty(dc.Name))
                .Should().BeNullOrEmpty();
        }
    }
}
