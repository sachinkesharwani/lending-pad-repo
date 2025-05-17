using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Orders
{
    public interface IUpdateOrderService
    {
        Order Update(Guid customerId, List<(Guid productId, int quantity)> items, OrderStatus status);
    }
}
