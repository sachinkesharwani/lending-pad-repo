using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Products
{
    public interface IGetProductService
    {
        Product Get(Guid id);
        IEnumerable<Product> Get(ProductCategory? category = null, string name = null, decimal? minPrice = null, decimal? maxPrice = null);
    }
}
