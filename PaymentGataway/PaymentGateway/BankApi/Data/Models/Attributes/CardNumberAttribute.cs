using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BankApi.Models.Attributes
{
    public class CardNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var valueStr = value.ToString();
            foreach (var reg in validNumbers.Values)
            {
                if (reg.IsMatch(valueStr))
                    return true;
            }
            ErrorMessage = "Номер карты неправиьный. Либо данная платёжная система не поддерживается.";
            return false;
        }

        private static Dictionary<string, Regex> validNumbers = new Dictionary<string, Regex>()
        {
            ["Мир"] = new Regex(@"2[0-9]{15}"),
            ["MasterCard"] = new Regex(@"5[0-9]{15}"),
            ["Maestro"] = new Regex(@"6[0-9]{15}"),
            ["Visa"] = new Regex(@"4[0-9]{12}(?:[0-9]{3})?")
        };
    }
}