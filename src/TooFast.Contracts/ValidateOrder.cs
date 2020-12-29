namespace TooFast.Contracts
{
    public record ValidateOrder
    {
        public OrderModel Order { get; init; }
    }
}