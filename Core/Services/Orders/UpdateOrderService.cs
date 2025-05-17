using BusinessEntities;
using Common;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Services.Orders
{
    [AutoRegister]
    public class UpdateOrderService : IUpdateOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public UpdateOrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public Order Update(Guid orderId, List<(Guid productId, int quantity)> items, OrderStatus status)
        {
            // Retrieve the existing order from the repository
            var order = _orderRepository.Get(orderId);

            if (order == null)
            {
                throw new Exception($"Order with ID {orderId} not found.");
            }

            // Clear existing items if we want to completely replace them
            // Otherwise, we can just add new items
            order.ClearItems(); // Assuming ClearItems is a method to remove current items, else modify as needed

            // Add new items to the order
            foreach (var item in items)
            {
                var product = _productRepository.Get(item.productId);
                if (product == null)
                {
                    throw new Exception($"Product with ID {item.productId} not found.");
                }

                order.AddProduct(product, item.quantity); // Add new product with quantity
            }

            // Update the order status if provided
            order.UpdateStatus(status);

            // Save the updated order back to the repository
            _orderRepository.Save(order);

            return order;
        }
    }
}
