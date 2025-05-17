using BusinessEntities;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class ProductRepository : InMemoryRepository<Product>, IProductRepository
    {
        private readonly IDictionary<Guid, Product> _productStorage;
        public ProductRepository()
        {
            _productStorage = new Dictionary<Guid, Product>();
        }
        public void Save(Product product)
        {
            if (!_productStorage.ContainsKey(product.Id))
            {
                _productStorage[product.Id] = product;  // This uses the Product's `Id`
            }
            else
            {
                _productStorage[product.Id] = product;  // Update if already exists
            }
        }
        public IEnumerable<Product> GetAll()
        {
            return _productStorage.Values.ToList();
        }
        public Product Get(Guid id)
        {
            if (_productStorage.TryGetValue(id, out var product))
            {
                return product;  // Return the product with the matching `Id`
            }
            return null;  // No product found with this ID
        }
        public void Delete(Product product)
        {
            if (_productStorage.ContainsKey(product.Id))
            {
                _productStorage.Remove(product.Id); // Remove the product from dictionary
            }
        }
        public IEnumerable<Product> Get(ProductCategory? category = null, string name = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            var products = _store.Values.AsEnumerable();

            if (category.HasValue)
                products = products.Where(p => p.Category == category.Value);

            if (!string.IsNullOrWhiteSpace(name))
                products = products.Where(p => !string.IsNullOrEmpty(p.Name) &&
                                               p.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);

            if (minPrice.HasValue)
                products = products.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                products = products.Where(p => p.Price <= maxPrice.Value);

            return products;
        }
    }
}
