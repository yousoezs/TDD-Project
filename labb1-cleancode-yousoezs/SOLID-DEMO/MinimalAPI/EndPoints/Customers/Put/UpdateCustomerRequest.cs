using Shared.Shared.DTOs;

namespace Server.MinimalAPI.EndPoints.Customers.Put
{
    public class UpdateCustomerRequest
    {
        public CustomerDto CustomerDto { get; set; }
    }
}
