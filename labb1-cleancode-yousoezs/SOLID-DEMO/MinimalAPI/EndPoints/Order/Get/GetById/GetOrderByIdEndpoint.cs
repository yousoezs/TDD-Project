using Domain.Services.DataHandlers;
using Domain.Services.Extensions;
using FastEndpoints;
using Shared.Shared.Interfaces;

namespace Server.MinimalAPI.EndPoints.Order.Get.GetById;

public class GetOrderByIdEndpoint : Endpoint<GetOrderByIdRequest, GetOrderByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetOrderByIdEndpoint(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public override void Configure()
    {
        Get("/getById/{id}");
        AllowAnonymous();
        Group<OrderApiGroup>();
    }

    public override async Task HandleAsync(GetOrderByIdRequest request, CancellationToken ct)
    {
        if (!Guid.TryParse(request.Id, out var guid)) return;

        var response = await OrderRepositoryDataHandler.OrderFetched(guid);
        if (!response.Success)
        {
            await SendAsync(new(), 400, ct);
            return;
        }

        if (response.Data is null)
        {
            if (!response.Success)
            {
                await SendAsync(new(), 204, ct);
                return;
            }
        }

        await SendAsync(new() { OrderDto = response.Data.ConvertToDto() }, 200, ct);
    }
}
