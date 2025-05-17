using Common;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Orders
{
    [AutoRegister]
    public class DeleteOrderService : IDeleteOrderService
    {
        private readonly IOrderRepository _repository;

        public DeleteOrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public void Delete(Guid id)
        {
            var order = _repository.Get(id);
            if (order != null)
            {
                _repository.Delete(order);
            }
        }
    }
}
