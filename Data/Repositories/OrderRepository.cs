using BusinessEntities;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repositories
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class OrderRepository : InMemoryRepository<Order>, IOrderRepository
    {
        // In-memory storage
        private readonly Dictionary<Guid, Order> _orderStorage;

        public OrderRepository()
        {
            // Initialize the in-memory storage for orders
            _orderStorage = new Dictionary<Guid, Order>();
        }

        public void Save(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            // Save the order to the in-memory storage
            if (_orderStorage.ContainsKey(order.Id))
            {
                _orderStorage[order.Id] = order; // Update existing order
            }
            else
            {
                _orderStorage.Add(order.Id, order); // Add new order
            }
        }

        public void Delete(Order order)
        {
            var delOrder = _orderStorage.Values.FirstOrDefault(o => o.OrderId == order.OrderId);
            {
                _orderStorage.Remove(delOrder.Id); // Remove the product from dictionary
            }
        }

        public Order Get(Guid orderId)
        {
            // Retrieve the order by its ID
            var order = _orderStorage.Values.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                return null;
            }
            else
            {
                return order;
            }// No product found with this ID
        }

        public IEnumerable<Order> GetAll()
        {
            // Return all orders
            return _orderStorage.Values.ToList();
        }

        public void DeleteAll()
        {
            // Clear all orders from the in-memory storage
            _orderStorage.Clear();
        }
    }

}
