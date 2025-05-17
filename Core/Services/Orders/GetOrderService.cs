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
    public class GetOrderService : IGetOrderService
    {
        private readonly IOrderRepository _repository;

        public GetOrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public Order Get(Guid id)
        {
            return _repository.Get(id);
        }

        public IEnumerable<Order> Get(ProductCategory? category = null, string name = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            var orders = _repository.GetAll(); // Get all orders from in-memory storage

            // Filter orders based on the properties of the products within each order
            return orders.Where(order =>
                order.Items.Any(item =>
                    (!category.HasValue || item.Product.Category == category.Value) &&
                    (string.IsNullOrEmpty(name) || item.Product.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0) &&
                    (!minPrice.HasValue || item.Product.Price >= minPrice.Value) &&
                    (!maxPrice.HasValue || item.Product.Price <= maxPrice.Value)
                )
            );
        }
    }
}
