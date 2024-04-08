using Domain.Services.DataHandlers;
using Domain.Services.Extensions;
using FastEndpoints;
using Shared.Shared.Interfaces;

namespace Server.MinimalAPI.EndPoints.Product.Put
{
    public class UpdateProductEndpoint : Endpoint<UpdateProductRequest, UpdateProductResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateProductEndpoint(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public override void Configure()
        {
            Put("/update");
            AllowAnonymous();
            Group<ProductApiGroup>();
        }

        public override async Task HandleAsync(UpdateProductRequest request, CancellationToken ct)
        {
            var response = await ProductRepositoryDataHandler.ProductUpdate(request.ProductDto.ConvertDtoToModel());
            if(!response.Success)
            {
                await SendAsync(new(), 400, ct);
                return;
            }

            await _unitOfWork.SaveAsync();
            await SendAsync(new() { ProductDto = response.Data.ConvertModelToDto() }, 200, ct);
        }
    }
}
