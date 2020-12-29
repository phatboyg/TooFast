namespace TooFast.Contracts
{
    using System;


    public record OrderInvalid
    {
        public Guid OrderId { get; init; }
    }
}