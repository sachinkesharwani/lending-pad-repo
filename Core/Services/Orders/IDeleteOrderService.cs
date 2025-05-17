using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Orders
{
    public interface IDeleteOrderService
    {
        void Delete(Guid id);
    }
}
