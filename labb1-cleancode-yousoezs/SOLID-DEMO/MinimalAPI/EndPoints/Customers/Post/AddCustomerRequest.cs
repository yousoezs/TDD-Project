using Shared.Shared.DTOs;

namespace Server.MinimalAPI.EndPoints.Customers.Post
{
    public class AddCustomerRequest
    {
        public CustomerDto CustomerDto { get; set; }
    }
}
