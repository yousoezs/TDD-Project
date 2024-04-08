using Domain.Services.DataHandlers;
using Domain.Services.Extensions;
using FastEndpoints;
using Shared.Shared.Interfaces;

namespace Server.MinimalAPI.EndPoints.Customers.Get.GetAllCustomers
{
    public class GetAllCustomersEndpoint : EndpointWithoutRequest<GetAllCustomersResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public GetAllCustomersEndpoint(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public override void Configure()
        {
            Get("/getAll");
            AllowAnonymous();
            Group<CustomerApiGroup>();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var response = await CustomerRepositoryDataHandler.FetchAllCustomer();
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

            await SendAsync(new() { Customers = response.Data.Select(c => c.ConvertToDto()) }, 200, ct);
        }
    }
}
