using System;
using System.ComponentModel.DataAnnotations;

namespace NexCart.Orders.Entities
{
    public class OrderItem
    {
        [Key]
        public Guid _id { get; set; }

        public Guid ProductID { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
