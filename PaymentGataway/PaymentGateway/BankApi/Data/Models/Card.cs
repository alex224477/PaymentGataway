using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BankApi.Models.Attributes;

namespace BankApi.Models
{
    public class Card
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CardId { get; set; }
        [Required]
        [CardNumber]
        public string CardNumber { get; set; }
        [Required]
        public int ExpiryMonth { get; set; }
        [Required]
        public int ExpiryYear { get; set; }
        [Required]
        public string CVV { get; set; }
        public string CardholderName { get; set; }
        [Required]
        public bool IsLimited { get; set; }
        [Required]
        public int Balance { get; set; }
        public int? LimitSum { get; set; }
        public List<Order> Orders { get; set; }
    }
}