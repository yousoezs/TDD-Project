using FastEndpoints;

namespace Server.MinimalAPI.EndPoints.Order.Get.GetById
{
    public class GetOrderByIdRequest
    {
        public string Id { get; set; } = null!;
    }
}
