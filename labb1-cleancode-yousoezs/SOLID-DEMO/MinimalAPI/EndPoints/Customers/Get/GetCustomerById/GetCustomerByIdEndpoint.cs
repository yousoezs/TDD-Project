using Domain.Services.DataHandlers;
using Domain.Services.Extensions;
using FastEndpoints;
using Shared.Shared.Interfaces;

namespace Server.MinimalAPI.EndPoints.Customers.Get.GetCustomerById
{
    public class GetCustomerByIdEndpoint : Endpoint<GetCustomerByIdRequest, GetCustomerByIdResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetCustomerByIdEndpoint(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public override void Configure()
        {
            Get("/getById/{id}");
            AllowAnonymous();
            Group<CustomerApiGroup>();
        }

        public override async Task HandleAsync(GetCustomerByIdRequest request, CancellationToken ct)
        {
            if (!Guid.TryParse(request.Id, out var guid)) return;

            var response = await CustomerRepositoryDataHandler.CustomerFetched(guid);

            if (!response.Success)
            {
                await SendAsync(new(), 400, ct);
                return;
            }

            await SendAsync(new () { CustomerDto = response.Data.ConvertToDto() }, 200, ct);
        }
    }
}
