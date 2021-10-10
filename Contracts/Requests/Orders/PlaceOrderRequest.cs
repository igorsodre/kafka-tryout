using System;

namespace Contracts.Requests.Orders
{
    public class PlaceOrderRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
