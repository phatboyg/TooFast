namespace TooFast.Contracts
{
    public record SubmitOrder
    {
        public OrderModel Order { get; init; }
    }
}