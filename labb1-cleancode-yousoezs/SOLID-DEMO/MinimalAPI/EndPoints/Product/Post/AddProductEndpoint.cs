using Domain.Services.DataHandlers;
using Domain.Services.Extensions;
using Domain.Services.Repositories;
using FastEndpoints;
using Shared.Shared.Interfaces;

namespace Server.MinimalAPI.EndPoints.Product.Post
{
    public class AddProductEndpoint : Endpoint<AddProductRequest, AddProductResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddProductEndpoint(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public override void Configure()
        {
            Post("/add");
            AllowAnonymous();
            Group<ProductApiGroup>();
        }

        public override async Task HandleAsync(AddProductRequest request, CancellationToken ct)
        {
            var response = await ProductRepositoryDataHandler.ProductCreation(request.ProductDto.ConvertDtoToModel());
            if(!response.Success)
            {
                await SendAsync(new(), 400, ct);
                return;
            }

            await _unitOfWork.SaveAsync();
            await SendAsync(new() { ProductDto = response.Data.ConvertModelToDto() }, 201, ct);
        }
    }
}
