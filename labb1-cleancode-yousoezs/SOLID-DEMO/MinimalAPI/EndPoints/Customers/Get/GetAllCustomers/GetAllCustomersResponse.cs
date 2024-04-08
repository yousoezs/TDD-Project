using Shared.Shared.DTOs;

namespace Server.MinimalAPI.EndPoints.Customers.Get.GetAllCustomers
{
    public class GetAllCustomersResponse
    {
        public IEnumerable<CustomerDto>? Customers { get; set; } 
    }
}
