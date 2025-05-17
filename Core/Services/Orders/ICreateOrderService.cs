using System;
using System.Collections.Generic;
using BusinessEntities;

namespace Core.Services.Users
{
    public interface ICreateOrderService
    {
        Order Create(Guid customerId, List<(Guid productId, int quantity)> items, OrderStatus status);
    }
}