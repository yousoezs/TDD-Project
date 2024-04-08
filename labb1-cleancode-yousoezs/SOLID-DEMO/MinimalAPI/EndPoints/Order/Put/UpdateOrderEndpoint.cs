using Domain.Services.DataHandlers;
using Domain.Services.Extensions;
using FastEndpoints;
using Shared.Shared.Interfaces;

namespace Server.MinimalAPI.EndPoints.Order.Put;
public class UpdateOrderEndpoint : Endpoint<UpdateOrderRequest, UpdateOrderResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    public UpdateOrderEndpoint(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public override void Configure()
    {
        Put("/update");
        AllowAnonymous();
        Group<OrderApiGroup>();
    }

    public override async Task HandleAsync(UpdateOrderRequest request, CancellationToken ct)
    {
        var response = await OrderRepositoryDataHandler.OrderUpdate(request.OrderDto.ConvertToModel());
        if (!response.Success)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await _unitOfWork.SaveAsync();
        await SendAsync(new() { OrderDto = response.Data.ConvertToDto() }, 201, ct);
    }
}
