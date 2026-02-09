using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NexCart.Orders.Entities
{
    
    public class Order
    {
        [Key]    
        public Guid _id { get; set; }

        public Guid OrderID { get; set; }

        public Guid UserID { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalBill { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}
