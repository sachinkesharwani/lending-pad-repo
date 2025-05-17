using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntities
{
    public class OrderItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public OrderItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"OrderItem(ProductId={Product.Id}, Quantity={Quantity}, Total={Product.GetPriceAfterDiscount() * Quantity})";
        }
    }
    public class Order : IdObject
    {
        public Guid OrderId { get; private set; }
        public Guid CustomerId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public OrderStatus Status { get; private set; }
        public List<OrderItem> Items { get; private set; }


        // Constructor to create a new order for a customer
        public Order(Guid customerId)
        {
            OrderId = Guid.NewGuid();  // Generate a new OrderId
            CustomerId = customerId;   // Set the provided CustomerId
            OrderDate = DateTime.UtcNow;  // Set current UTC time as OrderDate
            Status = OrderStatus.Pending;  // Default status
            Items = new List<OrderItem>(); // Initialize the items list
        }
        public Order() {
            Items = new List<OrderItem>();
        }
        public void ClearItems()
        {
            Items.Clear();
        }

        // Add a product to the order as an order item
        public void AddProduct(Product product, int quantity)
        {
            var orderItem = new OrderItem(product, quantity);
            Items.Add(orderItem);
        }

        // Update the order's status
        public void UpdateStatus(OrderStatus status)
        {
            Status = status;
        }

        // Method to update product details in an order item
        public void UpdateOrderItem(Guid productId, string name, decimal price, int stock, ProductCategory category)
        {
            var orderItem = Items.FirstOrDefault(item => item.Product.Id == productId);
            if (orderItem != null)
            {
                orderItem.Product.UpdateName(name);
                orderItem.Product.UpdatePrice(price);
                orderItem.Product.AddStock(stock);
                orderItem.Product.UpdateCategory(category);
            }
        }
        public override string ToString() =>
            $"Order(OrderId={OrderId}, CustomerId={CustomerId}, Status={Status}, OrderDate={OrderDate}, ItemsCount={Items.Count})";
    }
}
