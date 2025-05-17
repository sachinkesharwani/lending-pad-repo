using Core.Services.Orders;
using Core.Services.Products;
using Core.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using System.Web;
using WebApi.Models.Products;
using WebApi.Models.Orders;
using BusinessEntities;
using WebApi.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("orders")]
    public class OrderController : BaseApiController
    {
        private readonly ICreateOrderService _createOrderService;
        private readonly IUpdateOrderService _updateOrderService;
        private readonly IDeleteOrderService _deleteOrderService;
        private readonly IGetOrderService _getOrderService;
        private readonly IGetProductService _getProductService;

        public OrderController(
          ICreateOrderService createOrderService,
          IUpdateOrderService updateOrderService,
          IDeleteOrderService deleteOrderService,
          IGetProductService  getProductService,
          IGetOrderService getOrderService)
        {
            _createOrderService = createOrderService;
            _updateOrderService = updateOrderService;
            _deleteOrderService = deleteOrderService;
            _getOrderService = getOrderService;
            _getProductService = getProductService;
        }

        [Route("{orderId:guid}")]
        [HttpGet]
        public HttpResponseMessage GetOrder(Guid orderId)
        {
            // Retrieve the product from the repository using its ID
            var order = _getOrderService.Get(orderId);

            // If the product doesn't exist, return a "Not Found" response
            if (order == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Order Id not found.");
            }

            // Return the product data as the response
            var OrderData = new OrderData(order);  // Assuming you have a ProductData class that maps the product entity
            return Request.CreateResponse(HttpStatusCode.OK, OrderData);
        }
        [HttpPost]
        [Route("create")]
        public IHttpActionResult CreateOrder([FromBody] OrderCreateDto dto)
        {
            if (dto == null || dto.Items == null || !dto.Items.Any())
                return BadRequest("Invalid order data.");

            try
            {
                // Map DTO to tuple list
                var itemTuples = dto.Items
                    .Select(i => (i.ProductId, i.Quantity))
                    .ToList();

                var order = _createOrderService.Create(dto.CustomerId, itemTuples, OrderStatus.Pending);

                var orderData = new OrderData(order);
                return Created($"orders/{orderData.Id}", orderData);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [Route("{orderId:guid}/delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteOrder(Guid orderId)
        {
            // Retrieve the product from the repository using its ID
            var order = _getOrderService.Get(orderId);

            // If the product doesn't exist, return a "Not Found" response
            if (order == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Order not found.");
            }

            // Call the service to delete the product
            _deleteOrderService.Delete(order.OrderId);

            // Return a success response (200 OK) after deletion
            return Request.CreateResponse(HttpStatusCode.OK, "Order deleted successfully.");
        }
        [HttpPut]
        [Route("{orderId:guid}/update")]
        public IHttpActionResult UpdateOrder(Guid orderId, [FromBody] OrderCreateDto dto)
        {
            if (dto == null || dto.Items == null || !dto.Items.Any())
                return BadRequest("Invalid order data.");

            try
            {
                // Retrieve the existing order by ID
                var existingOrder = _getOrderService.Get(orderId);
                if (existingOrder == null)
                    return NotFound();  // Return 404 if the order doesn't exist

                // Map DTO to tuple list for items
                var itemTuples = dto.Items
                    .Select(i => (i.ProductId, i.Quantity))
                    .ToList();
                var order = _updateOrderService.Update(orderId, itemTuples, OrderStatus.Pending);

                // Return the updated order data
                var orderData = new OrderData(order);
                return Created($"orders/{orderData.Id}", orderData);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);  // Handle errors
            }
        }

    }
}