using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApi.Models
{
    public class OrderStatus
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderStatusId { get; set; }
        [Required]
        public string StatusName { get; set; }
        public List<Order> Orders {get; set; }
    }

    public enum OrderStatusEnum
    {
        Ready = 0,
        Completed = 1,
        Returned = 2
    }
}