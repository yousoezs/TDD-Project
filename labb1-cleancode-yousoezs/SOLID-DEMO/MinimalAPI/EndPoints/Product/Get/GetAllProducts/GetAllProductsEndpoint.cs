using Domain.Services.DataHandlers;
using Domain.Services.Extensions;
using FastEndpoints;
using Shared.Shared.Interfaces;

namespace Server.MinimalAPI.EndPoints.Product.Get.GetAllProducts
{
    public class GetAllProductsEndpoint : EndpointWithoutRequest<GetAllProductsResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllProductsEndpoint(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public override void Configure()
        {
            Get("/getAll");
            AllowAnonymous();
            Group<ProductApiGroup>();
        }   

        public override async Task HandleAsync(CancellationToken ct)
        {
            var response = await ProductRepositoryDataHandler.ProductsFetch();
            if(!response.Success)
            {
                await SendAsync(new(), 400, ct);
                return;
            }

            if(response.Data is null)
            {
                if (!response.Success)
                {
                    await SendAsync(new(), 204, ct);
                    return;
                }
            }

            await SendAsync(new() { Products = response.Data.Select(p => p.ConvertModelToDto()) }, 200, ct);
        }
    }
}
