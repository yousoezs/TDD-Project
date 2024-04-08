using Domain.Services.DataHandlers;
using FastEndpoints;
using Shared.Shared.Interfaces;

namespace Server.MinimalAPI.EndPoints.Product.Delete;
public class DeleteProductEndpoint : Endpoint<DeleteProductRequest, DeleteProductResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    public DeleteProductEndpoint(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public override void Configure()
    {
        Delete("/delete/{id}");
        AllowAnonymous();
        Group<ProductApiGroup>();
    }

    public override async Task HandleAsync(DeleteProductRequest request, CancellationToken ct)
    {
        if (!Guid.TryParse(request.Id, out var guid)) return;

        var response = await ProductRepositoryDataHandler.ProductDelete(guid);
        if (!response.Success)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await _unitOfWork.SaveAsync();

        await SendAsync(new(), 200, ct);
    }
}
