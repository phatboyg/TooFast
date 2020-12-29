namespace TooFast.FrontEnd.Components
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Contracts;


    public class ValidateOrderService :
        IValidateOrderService
    {
        readonly HttpClient _httpClient;

        public ValidateOrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task ValidateOrder(OrderModel order)
        {
            var response = await _httpClient.PostAsync("ValidateOrder", JsonContent.Create(order));

            response.EnsureSuccessStatusCode();
        }
    }
}