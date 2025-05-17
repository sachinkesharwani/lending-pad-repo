using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Orders
{
    public class OrderModel
    {
        public Guid CustomerId { get; set; }
        public List<OrderItemModel> Items { get; set; } = new List<OrderItemModel>();
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }

        // Constructor
        public OrderModel()
        {
        }

        // Constructor to initialize fields for easy object creation
        public OrderModel(Guid customerId, OrderStatus status, DateTime orderDate)
        {
            CustomerId = customerId;
            Status = status;
            OrderDate = orderDate;
        }
    }
}