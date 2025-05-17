using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Orders
{
    public class OrderItemModel
    {
        public Guid ProductId { get; set; }  // Product ID, instead of the full Product object
        public int Quantity { get; set; }

        // Constructor
        public OrderItemModel(Guid productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }
    }
}