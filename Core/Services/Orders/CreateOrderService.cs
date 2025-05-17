using BusinessEntities;
using Common;
using Core.Factories;
using Core.Services.Users;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services
{
    [AutoRegister]
    public class CreateOrderService : ICreateOrderService
    {
        private readonly IIdObjectFactory<Order> _orderFactory;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public CreateOrderService(
            IIdObjectFactory<Order> orderFactory,
            IOrderRepository orderRepository,
            IProductRepository productRepository)
        {
            _orderFactory = orderFactory;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public Order Create(Guid customerId, List<(Guid productId, int quantity)> items, OrderStatus status)
        {
            var order = new Order(customerId);

            foreach (var item in items)
            {
                var product = _productRepository.Get(item.productId);
                if (product == null)
                {
                    throw new Exception($"Product with ID {item.productId} not found.");
                }

                order.AddProduct(product, item.quantity);
            }

            order.UpdateStatus(status);
            _orderRepository.Save(order);

            return order;
        }

    }
}
