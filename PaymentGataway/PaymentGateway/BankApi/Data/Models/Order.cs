using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApi.Models
{
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderId { get; set; }
        [Required]
        [Range(0, 1000000, ErrorMessage = "Сумма заказа должна быть от 0 до 10000 рублей.")]
        public int OrderSum { get; set; }
        [Required]
        public int OrderStatusId { get; set; }
        [ForeignKey("OrderStatusId")]
        public OrderStatus OrderStatus { get; set; }
        public int? CardId { get; set; }
        [ForeignKey("CardId")]
        public Card Card { get; set; }
    }
}