using System;
using System.ComponentModel.DataAnnotations;

namespace BankApi.Models.Attributes
{
    public class ExpireDateAttribute : ValidationAttribute
    {
        public ExpireDateAttribute()
        {
            ErrorMessage = "Дата истечения срока карты не может быть меньше текущей.";
        }
        public override bool IsValid(object value)
        {
            var payment = value as Payment;
            DateTime expiryDate = new DateTime(payment.ExpiryYear, payment.ExpiryMonth, 1);
            if (DateTime.Now > expiryDate)
                return false;
            return true;
        }
    }
}