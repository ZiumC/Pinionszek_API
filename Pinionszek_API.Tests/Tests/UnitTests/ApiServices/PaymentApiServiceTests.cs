﻿using FluentAssertions;
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
        [Fact]
        public async Task PaymentsApiService_GetPaymentsAsync_ReturnsPaymentsOrNotfound()
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

        [Fact]
        public async Task PaymentsApiService_GetSharedPaymentDataAsync_ReturnsSharedPaymentOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var paymentApiService = new PaymentApiService(await dbContext);

            //Act
            var sharedIdPayment_1 = await paymentApiService.GetSharedPaymentDataAsync(1);
            var sharedIdPayment_4 = await paymentApiService.GetSharedPaymentDataAsync(4);
            var sharedIdPayment_10 = await paymentApiService.GetSharedPaymentDataAsync(10);
            var sharedNonExistingPayment = await paymentApiService.GetSharedPaymentDataAsync(100);

            //Assert
            sharedIdPayment_1.Should().NotBeNull();
            sharedIdPayment_1?.IdPayment.Should().Be(1);
            sharedIdPayment_1?.IdFriend.Should().Be(1);

            sharedIdPayment_4.Should().NotBeNull();
            sharedIdPayment_4?.IdPayment.Should().Be(4);
            sharedIdPayment_4?.IdFriend.Should().Be(1);

            sharedIdPayment_10.Should().NotBeNull();
            sharedIdPayment_10?.IdPayment.Should().Be(10);
            sharedIdPayment_10?.IdFriend.Should().Be(3);

            sharedNonExistingPayment.Should().BeNull();
        }

        [Fact]
        public async Task PaymentsApiService_GetAssignedPaymentsAsync_ReturnsPaymentsOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var paymentApiService = new PaymentApiService(await dbContext);

            //Act
            var assignedPaymentsFriendTag_1001 = await paymentApiService.GetAssignedPaymentsAsync(1001);
            var assignedPaymentsFriendTag_1002 = await paymentApiService.GetAssignedPaymentsAsync(1002);
            var assignedPaymentsFriendTag_1003 = await paymentApiService.GetAssignedPaymentsAsync(1003);
            var assignedPaymentsFriendTag_1004 = await paymentApiService.GetAssignedPaymentsAsync(1004);

            //Assert
            assignedPaymentsFriendTag_1001.Should().NotBeNullOrEmpty();
            assignedPaymentsFriendTag_1001.Should().BeOfType<List<Payment>>();
            assignedPaymentsFriendTag_1001.Count().Should().Be(1);
            assignedPaymentsFriendTag_1001
                .Where(ap => ap.SharedPayment == null)
                .ToList().Count().Should().Be(0);
            assignedPaymentsFriendTag_1001
                .Where(ap => ap.DetailedCategory == null ||
                    string.IsNullOrEmpty(ap.DetailedCategory.Name) ||
                    ap.DetailedCategory.IdGeneralCategory == 0 ||
                    ap.DetailedCategory.GeneralCategory.IdGeneralCategory == 0 ||
                    string.IsNullOrEmpty(ap.DetailedCategory?.GeneralCategory.Name))
                .ToList().Count().Should().Be(0);
            assignedPaymentsFriendTag_1001
                .Where(ap => ap.IdPaymentStatus == 0)
                .ToList().Count()
                .Should().Be(0);
            assignedPaymentsFriendTag_1001
                .Where(ap => string.IsNullOrEmpty(ap.Name))
                .ToList().Count().Should().Be(0);

            assignedPaymentsFriendTag_1002.Should().NotBeNullOrEmpty();
            assignedPaymentsFriendTag_1002.Should().BeOfType<List<Payment>>();
            assignedPaymentsFriendTag_1002.Count().Should().Be(2);
            assignedPaymentsFriendTag_1002
                .Where(ap => ap.SharedPayment == null)
                .ToList().Count().Should().Be(0);
            assignedPaymentsFriendTag_1002
                .Where(ap => ap.DetailedCategory == null ||
                    string.IsNullOrEmpty(ap.DetailedCategory.Name) ||
                    ap.DetailedCategory.IdGeneralCategory == 0 ||
                    ap.DetailedCategory.GeneralCategory.IdGeneralCategory == 0 ||
                    string.IsNullOrEmpty(ap.DetailedCategory?.GeneralCategory.Name))
                .ToList().Count().Should().Be(0);
            assignedPaymentsFriendTag_1002
                .Where(ap => ap.IdPaymentStatus == 0)
                .ToList().Count()
                .Should().Be(0);
            assignedPaymentsFriendTag_1002
                .Where(ap => string.IsNullOrEmpty(ap.Name))
                .ToList().Count().Should().Be(0);

            assignedPaymentsFriendTag_1003.Should().BeNullOrEmpty();
            assignedPaymentsFriendTag_1004.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task BudgetApiService_GetPaymentAsync_ReturnsPaymentOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var paymentApiService = new PaymentApiService(await dbContext);

            //Act
            var payment1 = await paymentApiService.GetPaymentAsync(1, 1);
            var payment2 = await paymentApiService.GetPaymentAsync(7, 2);
            var payment3 = await paymentApiService.GetPaymentAsync(2, 1);
            var payment4 = await paymentApiService.GetPaymentAsync(4, 2);
            var payment5 = await paymentApiService.GetPaymentAsync(13, 1);
            var payment6 = await paymentApiService.GetPaymentAsync(4, 1);

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
    }
}