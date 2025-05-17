using BusinessEntities;
using System;
using System.Collections.Generic;

namespace Data.Repositories
{
    public interface IProductRepository : IMemoryRepository<Product>
    {
        IEnumerable<Product> Get(ProductCategory? category = null, string name = null, decimal? minPrice = null, decimal? maxPrice = null);
        void Delete(Product product); // Delete product
        IEnumerable<Product> GetAll();
    }
}
