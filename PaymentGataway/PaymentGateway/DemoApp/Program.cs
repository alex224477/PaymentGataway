using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }
        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("http://localhost:59657/");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                Payment payment_for_return = new Payment
                {
                    OrderId = 1,
                    CardNumber = "4539804056541904",
                    ExpiryMonth = 12,
                    ExpiryYear = 2019,
                    CVV = "825",
                    CardholderName = "USER TEST",
                    AmountKop = 122000
                };
                Payment expensive_payment = new Payment
                {
                    OrderId = 3,
                    CardNumber = "4539804056541904",
                    ExpiryMonth = 12,
                    ExpiryYear = 2019,
                    CVV = "825",
                    CardholderName = "USER TEST",
                    AmountKop = 1250000
                };
                Console.WriteLine("Получение статуса заказа 1 до оплаты...");
                await GetOrderStatusAsync(1);
                Console.WriteLine("Отправка платежа с неправильным годом...");
                await PostPaymentAsync(payment_for_return);
                Console.WriteLine("Получение статуса заказа 1...");
                await GetOrderStatusAsync(1);
                payment_for_return.ExpiryYear = 2018;
                Console.WriteLine("Отправка платежа с правильным годом...");
                await PostPaymentAsync(payment_for_return);
                Console.WriteLine("Получение статуса заказа 1 после оплаты...");
                await GetOrderStatusAsync(1);
                Console.WriteLine("Возврат последнего платежа...");
                await RefundAsync(1);
                Console.WriteLine("Получение статуса заказа 1 после возврата...");
                await GetOrderStatusAsync(1);
                Console.WriteLine("Ещё одна попытка возврата последнего платежа...");
                await RefundAsync(1);
                Console.WriteLine("Попытка оплаты дорогой покупки лимитной картой...");
                await PostPaymentAsync(expensive_payment);
                expensive_payment.CardNumber = "5190967013969042";
                expensive_payment.ExpiryMonth = 5;
                expensive_payment.ExpiryYear = 2021;
                expensive_payment.CVV = "627";
                expensive_payment.CardholderName = null;
                Console.WriteLine("Попытка оплаты дорогой покупки безлимитной картой...");
                await PostPaymentAsync(expensive_payment);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }

        static async Task PostPaymentAsync(Payment payment)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync($"pay", payment);
            Console.WriteLine("Код: " + response.StatusCode);
        }

        static async Task RefundAsync(int order_id)
        {
            HttpResponseMessage response = await client.PutAsync($"refund/{order_id}", null);
            Console.WriteLine("Код: " + response.StatusCode);
        }

        static async Task GetOrderStatusAsync(int order_id)
        {
            HttpResponseMessage response = await client.GetAsync($"status/{order_id}");
            var status = await response.Content.ReadAsAsync<string>();
            Console.WriteLine($"Код: {response.StatusCode}. Статус платежа для заказа {order_id}: {status}");
        }
    }
}
