using BankApi.Controllers;
using BankApi.Models;
using BankApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace BankApiUnitTest
{
    public class PaymentControllerTests
    {
        [Fact]
        public void PaymentPost()
        {
            var serviceMock = new Mock<IGatewayService>();
            var loggerMock = new Mock<ILogger<PaymentController>>();
            string cardNumber = "4539804056541954";
            int expiryMonth = 12;
            int expiryYear = 2020;
            string CVV = "053";
            int low_amount_order_id = 1;
            int high_amount_order_id = 2;
            int completed_order_id = 3;

            Payment payment1 = new Payment
            {
                OrderId = low_amount_order_id,
                CardNumber = cardNumber,
                ExpiryMonth = expiryMonth,
                ExpiryYear = expiryYear,
                CVV = CVV,
                AmountKop = 25001
            };

            Payment payment2 = new Payment
            {
                OrderId = high_amount_order_id,
                CardNumber = cardNumber,
                ExpiryMonth = expiryMonth,
                ExpiryYear = expiryYear,
                CVV = CVV,
                AmountKop = 125000
            };

            serviceMock.Setup(gs => gs.FindCard(cardNumber, expiryMonth, expiryYear, CVV, null)).Returns(GetCard());
            serviceMock.Setup(gs => gs.GetOrderById(low_amount_order_id)).Returns(GetLowAmountOrder());
            serviceMock.Setup(gs => gs.GetOrderById(high_amount_order_id)).Returns(GetHighAmountOrder());
            serviceMock.Setup(gs => gs.GetOrderById(completed_order_id)).Returns(GetCompletedOrder());

            var controller = new PaymentController(serviceMock.Object, loggerMock.Object);
            var result = controller.PaymentPost(payment1);
            var wrongSumResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Сумма платежа не совпадает с суммой заказа.", wrongSumResult.Value);

            payment1.AmountKop = 25000;
            result = controller.PaymentPost(payment1);
            var okResult = Assert.IsType<OkResult>(result);

            payment1.OrderId = 3;
            result = controller.PaymentPost(payment1);
            var alreadyPaidesult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Заказ не может быть оплачен.", alreadyPaidesult.Value);

            result = controller.PaymentPost(payment2);
            var notEnoughMoneyResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Недостаточно средств.", notEnoughMoneyResult.Value);
        }

        private Order GetLowAmountOrder()
        {
            return new Order() { OrderId = 1, OrderSum = 25000, OrderStatusId = ((int)OrderStatusEnum.Ready) };
        }

        private Order GetHighAmountOrder()
        {
            return new Order() { OrderId = 2, OrderSum = 125000, OrderStatusId = ((int)OrderStatusEnum.Ready) };
        }

        private Order GetCompletedOrder()
        {
            return new Order() { OrderId = 1, OrderSum = 25000, OrderStatusId = ((int)OrderStatusEnum.Completed) };
        }

        private Card GetCard()
        {
            return new Card()
            {
                CardId = 1,
                CardNumber = "4539804056541954",
                ExpiryMonth = 12,
                ExpiryYear = 2020,
                CVV = "053",
                CardholderName = null,
                IsLimited = true,
                Balance = 100000,
                LimitSum = 0
            };
        }
    }
}
