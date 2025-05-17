using BusinessEntities;
using Common;
using Core.Factories;
using Core.Services.Users;
using Data.Repositories;
using System;

[AutoRegister]
public class CreateProductService : ICreateProductService
{
    private readonly IIdObjectFactory<Product> _productFactory;
    private readonly IProductRepository _productRepository;
    
    public CreateProductService(IIdObjectFactory<Product> productFactory, IProductRepository productRepository)
    {
        _productFactory = productFactory;
        _productRepository = productRepository;
    }

    public Product Create(Guid id, string name, decimal price, int stock, ProductCategory category)
    {
        var product = _productFactory.Create(id);
        product.UpdateName(name);
        product.UpdatePrice(price);
        product.AddStock(stock);
        product.UpdateCategory(category);
        _productRepository.Save(product);
        return product;
    }
}
