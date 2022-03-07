using System;

namespace API.Domain;

public class Order
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}