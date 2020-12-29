namespace TooFast.Contracts
{
    using System;


    public record OrderRejected
    {
        public Guid OrderId { get; init; }
    }
}