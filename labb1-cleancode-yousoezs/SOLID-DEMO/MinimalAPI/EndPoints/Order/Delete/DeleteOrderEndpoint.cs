using Domain.Services.DataHandlers;
using FastEndpoints;
using Shared.Shared.Interfaces;

namespace Server.MinimalAPI.EndPoints.Order.Delete;

public class DeleteOrderEndpoint : Endpoint<DeleteOrderRequest, DeleteOrderResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    public DeleteOrderEndpoint(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    public override void Configure()
    {
        Delete("/delete/{id}");
        AllowAnonymous();
        Group<OrderApiGroup>();
    }

    public override async Task HandleAsync(DeleteOrderRequest request, CancellationToken ct)
    {
        if (!Guid.TryParse(request.Id, out var guid)) return;

        var response = await OrderRepositoryDataHandler.OrderDeletion(guid);
        if (!response.Success)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await _unitOfWork.SaveAsync();
        await SendAsync(new(), 200, ct);
    }
}
