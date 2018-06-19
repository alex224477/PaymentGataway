using System.Linq;

namespace BankApi.Models
{
    public static class DbSeeder
    {
        public static void Seed(GatawayDbContext dbContext)
        {
            dbContext.Cards.RemoveRange(dbContext.Cards);
            dbContext.Orders.RemoveRange(dbContext.Orders);
            dbContext.SaveChanges();
            if (!dbContext.Cards.Any())
            {
                CreateCards(dbContext);
            }
            if (!dbContext.OrderStatuses.Any())
            {
                CreateOrderStatuses(dbContext);
            }
            if (!dbContext.Orders.Any())
            {
                CreateOrders(dbContext);
            }
        }

        private static void CreateCards(GatawayDbContext dbContext)
        {
            var limitedCard = new Card()
            {
                CardId = 1,
                CardNumber = "4539804056541904",
                ExpiryMonth = 12,
                ExpiryYear = 2018,
                CVV = "825",
                CardholderName = "USER TEST",
                IsLimited = true,
                Balance = 123000,
                LimitSum = -100000
            };
            var unlimitedCard = new Card()
            {
                CardId = 2,
                CardNumber = "5190967013969042",
                ExpiryMonth = 5,
                ExpiryYear = 2021,
                CVV = "627",
                IsLimited = false,
                Balance = 222000
            };

            dbContext.Cards.Add(limitedCard);
            dbContext.Cards.Add(unlimitedCard);
            dbContext.SaveChanges();
        }

        private static void CreateOrderStatuses(GatawayDbContext dbContext)
        {
            var readyStatus = new OrderStatus() { OrderStatusId = (int)OrderStatusEnum.Ready, StatusName = "Не сделан" };
            var completedStatus = new OrderStatus() { OrderStatusId = (int)OrderStatusEnum.Completed, StatusName = "Cделан" };
            var returnedStatus = new OrderStatus() { OrderStatusId = (int)OrderStatusEnum.Returned, StatusName = "Возвращён" };

            dbContext.OrderStatuses.Add(readyStatus);
            dbContext.OrderStatuses.Add(completedStatus);
            dbContext.OrderStatuses.Add(returnedStatus);
            dbContext.SaveChanges();
        }

        private static void CreateOrders(GatawayDbContext dbContext)
        {
            var orders1 = new Order() { OrderId = 1, OrderSum = 122000, OrderStatus = dbContext.OrderStatuses.Find((int)OrderStatusEnum.Ready) };
            var orders2 = new Order() { OrderId = 2, OrderSum = 125000, OrderStatus = dbContext.OrderStatuses.Find((int)OrderStatusEnum.Ready) };
            var orders3 = new Order() { OrderId = 3, OrderSum = 1250000, OrderStatus = dbContext.OrderStatuses.Find((int)OrderStatusEnum.Ready) };

            dbContext.Orders.Add(orders1);
            dbContext.Orders.Add(orders2);
            dbContext.Orders.Add(orders3);
            dbContext.SaveChanges();
        }
    }
}
