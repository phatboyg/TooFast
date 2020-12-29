namespace TooFast.Contracts
{
    using System;


    public record OrderValidated
    {
        public Guid OrderId { get; init; }
    }
}