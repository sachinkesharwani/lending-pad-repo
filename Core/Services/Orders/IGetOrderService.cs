using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Orders
{
    public interface IGetOrderService
    {
        Order Get(Guid id);
        IEnumerable<Order> Get(ProductCategory? category = null, string name = null, decimal? minPrice = null, decimal? maxPrice = null);
    }
}
