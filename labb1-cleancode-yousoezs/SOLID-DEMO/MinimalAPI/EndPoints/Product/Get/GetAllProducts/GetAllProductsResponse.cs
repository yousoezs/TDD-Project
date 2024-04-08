using Shared.Shared.DTOs;

namespace Server.MinimalAPI.EndPoints.Product.Get.GetAllProducts
{
    public class GetAllProductsResponse
    {
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
