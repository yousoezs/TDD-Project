using Domain.Services.DataHandlers;
using Domain.Services.Extensions;
using FastEndpoints;
using Shared.Shared.Interfaces;

namespace Server.MinimalAPI.EndPoints.Order.Post;

public class AddOrderEndpoint : Endpoint<AddOrderRequest, AddOrderResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    public AddOrderEndpoint(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public override void Configure()
    {
        Post("/add");
        AllowAnonymous();
        Group<OrderApiGroup>();
    }

    public override async Task HandleAsync(AddOrderRequest request, CancellationToken ct)
    {
        var response = await OrderRepositoryDataHandler.OrderCreation(request.OrderDto.ConvertToModel());
        if(!response.Success)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        await _unitOfWork.SaveAsync();
        await SendAsync(new() { OrderDto = response.Data.ConvertToDto() }, 201, ct);
    }
}
