using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain;
using API.Kafka;
using Contracts.Requests.Orders;
using Contracts.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IEventProducer<Order> _eventProducer;

        public OrdersController(IEventProducer<Order> eventProducer)
        {
            _eventProducer = eventProducer;
        }

        // POST: api/Orders
        [HttpPost]
        [Route("place-order")]
        public async Task<IActionResult> PlaceOrder(PlaceOrderRequest request)
        {
            var order = new Order { Quantity = request.Quantity, ProductId = request.ProductId };

            var result = await _eventProducer.ProduceAsync(order);
            if (!result.Success)
            {
                return BadRequest(new ErrorResponse(result.ErrorMessages));
            }

            return Ok(SuccessResponse.DefaultOkResponse());
        }
    }
}
