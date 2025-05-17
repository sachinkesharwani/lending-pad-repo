using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Orders
{
    public class OrderItemData
    {
        public OrderItemData(OrderItem item)
        {
            ProductId = item.Product.Id;
            ProductName = item.Product.Name;  // Assuming the Product object has Name property
            Quantity = item.Quantity;
            Price = item.Product.Price;
            SubTotal = item.Quantity * item.Product.Price;  // Price * Quantity for this item
        }

        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get; set; }  // Total price for this item (Price * Quantity)
    }
}