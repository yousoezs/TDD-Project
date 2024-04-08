using Shared.Shared.DTOs;

namespace Server.MinimalAPI.EndPoints.Order.Get.GetById
{
    public class GetOrderByIdResponse
    {
        public OrderDto? OrderDto { get; set; }
    }
}
