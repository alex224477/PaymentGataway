using BankApi.Models;
using BankApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace BankApi.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IGatewayService _dataService;
        private readonly ILogger _logger;

        public PaymentController(IGatewayService dataService, ILogger<PaymentController> logger)
        {
            _dataService = dataService;
            _logger = logger;
        }

        /// <summary>
        /// Оплата товара или услуги.
        /// </summary>
        [HttpPost]
        [Route("pay")]
        public IActionResult PaymentPost([FromBody]Payment payment)
        {
            if (payment == null)
                return BadRequest("Нет данных.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                Card card = _dataService.FindCard(payment.CardNumber, payment.ExpiryMonth,
                                                 payment.ExpiryYear, payment.CVV, payment.CardholderName);
                if (card == null)
                    return BadRequest("Карта не найдена.");
                if (card.IsLimited && card.Balance - payment.AmountKop < card.LimitSum.Value)
                    return BadRequest("Недостаточно средств.");

                Order order = _dataService.GetOrderById(payment.OrderId);

                if (order == null)
                    return BadRequest("Заказ не найден.");
                if (order.OrderSum != payment.AmountKop)
                    return BadRequest("Сумма платежа не совпадает с суммой заказа.");
                if (order.OrderStatusId != (int)OrderStatusEnum.Ready)
                    return BadRequest("Заказ не может быть оплачен.");

                _dataService.WriteOff(card, payment, order);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Проверка статуса транзакции.
        /// </summary>
        [HttpGet]
        [Route("status/{order_id?}")]
        public IActionResult OrderStatusGet(int order_id)
        {
            try
            {
                Order order = _dataService.GetOrderById(order_id);

                if (order == null)
                    return BadRequest("Заказ не найден.");
                
                return Ok(_dataService.GetOrderStatusById(order.OrderStatusId).StatusName);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Возврат.
        /// </summary>
        [HttpPut]
        [Route("refund/{order_id}")]
        public IActionResult Refund(int order_id)
        {
            try
            {
                Order order = _dataService.GetOrderById(order_id);

                if (order == null)
                    return BadRequest("Заказ не найден.");
                if (order.OrderStatusId != (int)OrderStatusEnum.Completed)
                    return BadRequest("Возврат заказа невозможен.");

                Card card = _dataService.GetCardById(order.CardId.Value);
                if (card == null)
                    return BadRequest("Карта не найдена.");

                _dataService.Return(card, order);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}