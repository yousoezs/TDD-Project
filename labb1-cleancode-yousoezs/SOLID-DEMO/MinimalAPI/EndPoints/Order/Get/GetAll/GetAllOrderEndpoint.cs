using Domain.Services.DataHandlers;
using Domain.Services.Extensions;
using FastEndpoints;
using Server.MinimalAPI.EndPoints.Customers;
using Shared.Shared.Interfaces;

namespace Server.MinimalAPI.EndPoints.Order.Get.GetAll;

public class GetAllOrderEndpoint : EndpointWithoutRequest<GetAllOrderResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllOrderEndpoint(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    public override void Configure()
    {
        Get("/getAll");
        AllowAnonymous();
        Group<OrderApiGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await OrderRepositoryDataHandler.FetchAllOrder();
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

        await SendAsync(new() { OrderDtos = response.Data.Select(c => c.ConvertToDto()) }, 200, ct);
    }
}
