using BankApi.Models;

namespace BankApi.Services.Interfaces
{
    public interface IGatewayService
    {
        Card FindCard(string cardNumber, int expiryMonth, int expiryYear, string CVV, string cardholderName);
        Order GetOrderById(int orderId);
        void WriteOff(Card card, Payment payment, Order order);
        Card GetCardById(int cardId);
        OrderStatus GetOrderStatusById(int orderstatusId);
        void Return(Card card, Order order);
    }
}