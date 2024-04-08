using Shared.Shared.DTOs;

namespace Server.MinimalAPI.EndPoints.Order.Put
{
    public class UpdateOrderRequest
    {
        public OrderDto OrderDto { get; set; }
    }
}
