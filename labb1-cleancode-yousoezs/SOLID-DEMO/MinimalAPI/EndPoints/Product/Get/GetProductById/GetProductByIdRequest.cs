using FastEndpoints;
using Shared.Shared.DTOs;

namespace Server.MinimalAPI.EndPoints.Product.Get.GetProductById
{
    public class GetProductByIdRequest
    {
        public string Id { get; set; } = null!;
    }
}
