namespace TooFast.Contracts
{
    using System;


    public record ProcessOrder
    {
        public Guid OrderId { get; init; }
    }
}