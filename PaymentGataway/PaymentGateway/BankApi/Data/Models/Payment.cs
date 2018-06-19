using System.ComponentModel.DataAnnotations;
using BankApi.Models.Attributes;

namespace BankApi.Models
{
    [ExpireDate]
   public class Payment
    {
        //public int PaymentId { get; set; }

        [Required(ErrorMessage = "Не получен идентификатор заказа.")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Не указан номер карты.")]
        [CardNumber]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Не указан месяц истечения срока действия карты.")]
        [Range(1, 12, ErrorMessage = "Недопустимое значение месяца.")]
        public int ExpiryMonth { get; set; }

        [Required(ErrorMessage = "Не указан год истечения срока действия карты.")]
        public int ExpiryYear { get; set; }

        [Required(ErrorMessage = "Не указан верификационный код")]
        [RegularExpression(@"\d{3}", ErrorMessage = "Неверный формат верификационного кода")]
        public string CVV { get; set; }

        public string CardholderName { get; set; }

        [Required(ErrorMessage = "Не указана сумма")]
        [Range(0, uint.MaxValue, ErrorMessage = "Сумма дожна быть больше нуля")]
        public uint AmountKop { get; set; }
    }
}