using BusinessEntities;
using Common;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Products
{
    [AutoRegister]
    public class UpdateProductService : IUpdateProductService
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void UpdateProduct(Guid productId, string name, decimal price, int stock, ProductCategory category)
        {
            var product = _productRepository.Get(productId);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            product.UpdateName(name);
            product.UpdatePrice(price);
            product.AddStock(stock);
            product.UpdateCategory(category);

            // Save the updated product back to the repository
            _productRepository.Save(product);
        }
    }
}
