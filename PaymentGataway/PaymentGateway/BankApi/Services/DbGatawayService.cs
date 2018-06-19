using System.Collections.Generic;
using System.Linq;
using BankApi.Models;
using BankApi.Services.Interfaces;

namespace BankApi.Services
{
    public class DbGatawayService : IGatewayService
    {
        private readonly GatawayDbContext _context;
        public DbGatawayService(GatawayDbContext context)
        {
            _context = context;
        }

        public Card FindCard(string cardNumber, int expiryMonth, int expiryYear, string CVV, string cardholderName)
        {
            return _context.Cards.FirstOrDefault(c => c.CardNumber == cardNumber 
                                  && c.ExpiryMonth == expiryMonth 
                                  && c.ExpiryYear == expiryYear 
                                  && c.CVV == CVV
                                  && c.CardholderName == cardholderName);
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders.Find(orderId);
        }

        public Card GetCardById(int cardId)
        {
            return _context.Cards.Find(cardId);
        }

        public void WriteOff(Card card, Payment payment, Order order)
        {
            card.Balance -= (int)payment.AmountKop;
            order.OrderStatusId = (int)OrderStatusEnum.Completed;
            order.Card = card;
            _context.SaveChanges();
        }

        public OrderStatus GetOrderStatusById(int orderstatusId)
        {
            return _context.OrderStatuses.Find(orderstatusId);
        }

        public void Return(Card card, Order order)
        {
            card.Balance += order.OrderSum;
            order.OrderStatusId = (int)OrderStatusEnum.Returned;
            _context.SaveChanges();
        }
    }
}