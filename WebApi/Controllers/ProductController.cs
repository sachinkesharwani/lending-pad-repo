using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessEntities;
using Core.Services.Products;
using Core.Services.Users;
using Data.Repositories;
using WebApi.Models.Products;

namespace WebApi.Controllers
{
    [RoutePrefix("products")]
    public class ProductController : BaseApiController
    {
        private readonly ICreateProductService _createProductService;
        private readonly IUpdateProductService _updateProductService;
        private readonly IDeleteProductService _deleteProductService;
        private readonly IGetProductService _getProductService;


        public ProductController(
           ICreateProductService createProductService,
           IUpdateProductService updateProductService,
           IDeleteProductService deleteProductService,
           IGetProductService getProductService)
        {
            _createProductService = createProductService;
            _updateProductService = updateProductService;
            _deleteProductService = deleteProductService;
            _getProductService = getProductService;
        }
        [Route("{productId:guid}")]
        [HttpGet]
        public HttpResponseMessage GetProduct(Guid productId)
        {
            // Retrieve the product from the repository using its ID
            var product = _getProductService.Get(productId);

            // If the product doesn't exist, return a "Not Found" response
            if (product == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product not found.");
            }

            // Return the product data as the response
            var productData = new ProductData(product);  // Assuming you have a ProductData class that maps the product entity
            return Request.CreateResponse(HttpStatusCode.OK, productData);
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage CreateProduct([FromBody] ProductModel model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            if (model.Id == Guid.Empty)
                model.Id = Guid.NewGuid(); // Generate new ID if not provided
            var existingProduct = _getProductService.Get(model.Id);
            if (existingProduct != null)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict, "Product already exists.");
            }
           
            
            try
            {
                var product = _createProductService.Create(
                  model.Id,      // This can either be an existing ID or leave it as default if the ID is auto-generated.
                  model.Name,    // Product name
                  model.Price,   // Product price
                  model.Stock,   // Stock quantity
                  model.Category // Product category
                );
                // Simulated save logic — since you're using in-memory, you may not need real transaction logic
                return Request.CreateResponse(HttpStatusCode.Created, product);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.Conflict, "Product creation failed due to concurrency.");
            }
        }
        [Route("update")]
        [HttpPost]
        public HttpResponseMessage UpdateProduct([FromBody] ProductModel model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var product = _getProductService.Get(model.Id);
            if (product == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Product does not exist.");
            }

            _updateProductService.UpdateProduct(model.Id, model.Name, model.Price, model.Stock, model.Category);

            // Assuming ProductData is a view model or data representation
            return Request.CreateResponse(HttpStatusCode.OK, new ProductData(product));
        }
        [Route("{productId:guid}/delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteProduct(Guid productId)
        {
            // Retrieve the product from the repository using its ID
            var product = _getProductService.Get(productId);

            // If the product doesn't exist, return a "Not Found" response
            if (product == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product not found.");
            }

            // Call the service to delete the product
            _deleteProductService.Delete(product.Id);

            // Return a success response (200 OK) after deletion
            return Request.CreateResponse(HttpStatusCode.OK, "Product deleted successfully.");
        }
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllProducts(string name = null,string category = null,decimal? minPrice = null,decimal? maxPrice = null)
        {
            ProductCategory? productCategory = null;
            if (!string.IsNullOrEmpty(category) && Enum.TryParse(category, true, out ProductCategory parsedCategory))
            {
                productCategory = parsedCategory;
            }
            var products = _getProductService.Get(productCategory, name, minPrice, maxPrice);
            if (!products.Any())
                return NotFound();

            return Ok(products.Select(p => new ProductData(p)));
        }




    }
}