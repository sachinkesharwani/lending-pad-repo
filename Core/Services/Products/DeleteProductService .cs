using Common;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Products
{
    [AutoRegister]
    public class DeleteProductService : IDeleteProductService
    {
        private readonly IProductRepository _repository;

        public DeleteProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public void Delete(Guid id)
        {
            var product = _repository.Get(id);
            if (product != null)
            {
                _repository.Delete(product);
            }
        }
    }
}
