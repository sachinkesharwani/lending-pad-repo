using BusinessEntities;
using Common;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Services.Products
{
    [AutoRegister]
    public class GetProductService : IGetProductService
    {
        private readonly IProductRepository _repository;

        public GetProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        // Get a specific product by ID
        public Product Get(Guid id)
        {
            return _repository.Get(id);
        }

        // Get filtered products based on category, name, price range
        public IEnumerable<Product> Get(ProductCategory? category = null, string name = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            // Get all products from the repository (assuming in-memory storage)
            var products = _repository.GetAll(); // Get all products from the dictionary

            // Apply filtering based on category, name, minPrice, maxPrice
            if (category.HasValue)
            {
                products = products.Where(p => p.Category == category.Value).ToList();
            }

            if (!string.IsNullOrEmpty(name))
            {
                // Filter by product name (case-insensitive)
                products = products.Where(p => p.Name != null && p.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            if (minPrice.HasValue)
            {
                // Filter by minimum price
                products = products.Where(p => p.Price >= minPrice.Value).ToList();
            }

            if (maxPrice.HasValue)
            {
                // Filter by maximum price
                products = products.Where(p => p.Price <= maxPrice.Value).ToList();
            }

            return products;
        }

    }
}
