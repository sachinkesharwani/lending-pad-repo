using System;
using System.Collections.Generic;
using BusinessEntities;

namespace Data.Repositories
{
    public interface IOrderRepository : IMemoryRepository<Order>
    {
        // Save the order
        void Save(Order order);
        void Delete(Order order); // Delete Order
    }
}