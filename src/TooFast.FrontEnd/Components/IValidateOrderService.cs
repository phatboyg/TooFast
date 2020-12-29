namespace TooFast.FrontEnd.Components
{
    using System.Threading.Tasks;
    using Contracts;


    public interface IValidateOrderService
    {
        Task ValidateOrder(OrderModel order);
    }
}