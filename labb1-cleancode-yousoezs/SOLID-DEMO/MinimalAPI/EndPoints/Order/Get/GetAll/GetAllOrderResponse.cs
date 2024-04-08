using Shared.Shared.DTOs;

namespace Server.MinimalAPI.EndPoints.Order.Get.GetAll;

public class GetAllOrderResponse
{
    public IEnumerable<OrderDto> OrderDtos { get; set; }
}
