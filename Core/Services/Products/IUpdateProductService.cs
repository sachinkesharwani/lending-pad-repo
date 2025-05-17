using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Products
{
    public interface IUpdateProductService
    {
        void UpdateProduct(Guid productId, string name, decimal price, int stock, ProductCategory category);
    }
}
