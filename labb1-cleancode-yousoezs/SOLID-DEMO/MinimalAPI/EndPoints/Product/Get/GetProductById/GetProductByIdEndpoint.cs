using Domain.Services.DataHandlers;
using Domain.Services.Extensions;
using FastEndpoints;
using Shared.Shared.Interfaces;

namespace Server.MinimalAPI.EndPoints.Product.Get.GetProductById
{
    public class GetProductByIdEndpoint : Endpoint<GetProductByIdRequest, GetProductByIdResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetProductByIdEndpoint(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public override void Configure()
        {
            Get("/getById/{id}");
            AllowAnonymous();
            Group<ProductApiGroup>();
        }

        public override async Task HandleAsync(GetProductByIdRequest request, CancellationToken ct)
        {
            if (!Guid.TryParse(request.Id, out var guid)) return;

            var response = await ProductRepositoryDataHandler.ProductFetched(guid);
            if (!response.Success)
            {
                await SendAsync(new(), 400, ct);
                return;
            }

            await SendAsync(new() { ProductDto = response.Data.ConvertModelToDto() }, 200, ct);
        }
    }
}
