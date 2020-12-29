namespace TooFast.Contracts
{
    using System;


    public record OrderSubmitted
    {
        public Guid OrderId { get; init; }
    }
}