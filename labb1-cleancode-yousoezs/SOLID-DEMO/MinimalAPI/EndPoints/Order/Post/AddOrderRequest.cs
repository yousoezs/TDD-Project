using Shared.Shared.DTOs;

namespace Server.MinimalAPI.EndPoints.Order.Post
{
    public class AddOrderRequest
    {
        public OrderDto OrderDto { get; set; }
    }
}
