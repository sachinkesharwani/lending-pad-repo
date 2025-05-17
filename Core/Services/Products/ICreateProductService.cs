using System;
using System.Collections.Generic;
using BusinessEntities;

namespace Core.Services.Users
{
    public interface ICreateProductService
    {
        Product Create(Guid id, string name, decimal price, int stock, ProductCategory category);
    }
}