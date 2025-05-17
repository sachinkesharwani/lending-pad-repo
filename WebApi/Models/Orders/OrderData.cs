using System;
using System.Collections.Generic;
using System.Linq;
using BusinessEntities;
using WebApi.Models.Orders;

namespace WebApi.Models
{
    // OrderData class to present Order details in response
    public class OrderData : IdObjectData
    {
        public OrderData(Order order) : base(order)
        {
            // Initialize properties from the 'order' object
            CustomerId = order.CustomerId;
            Id = order.OrderId;
            OrderDate = order.OrderDate;
            Status = order.Status.ToString();  // Convert Enum to string for easier use on the frontend
            TotalPrice = order.Items.Sum(item => item.Product.Price * item.Quantity);  // Calculate total price
            Items = order.Items.Select(item => new OrderItemData(item)).ToList(); // Mapping order items to OrderItemData
        }

        // OrderData Properties
        public Guid CustomerId { get; set; }  // Customer ID
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }  // Enum converted to string (Pending, Shipped, etc.)
        public decimal TotalPrice { get; set; }
        public List<OrderItemData> Items { get; set; }  // List of items in the order
    }

    public class OrderCreateDto
    {
        public Guid CustomerId { get; set; }  // Customer ID who is placing the order
        public List<OrderItemDto> Items { get; set; }  // List of order items (with ProductId and Quantity)
    }
    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
