namespace TooFast.Contracts
{
    using System;


    public record OrderModel
    {
        public Guid OrderId { get; init; }
        public decimal Total { get; init; }
    }
}