using Domain.Services.DataHandlers;
using Domain.Services.Extensions;
using FastEndpoints;
using Shared.Shared.DTOs;
using Shared.Shared.Interfaces;

namespace Server.MinimalAPI.EndPoints.Customers.Put
{
    public class UpdateCustomerEndpoint : Endpoint<UpdateCustomerRequest, UpdateCustomerResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCustomerEndpoint(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public override void Configure()
        {
            Put("/update");
            AllowAnonymous();
            Group<CustomerApiGroup>();
        }

        public override async Task HandleAsync(UpdateCustomerRequest request, CancellationToken ct)
        {
            var response = await CustomerRepositoryDataHandler.CustomerUpdate(request.CustomerDto.ConvertToModel());
            if(!response.Success)
            {
                await SendAsync(new(), 400, ct);
                return;
            }

            await _unitOfWork.SaveAsync();
            await SendAsync(new() { CustomerDto = response.Data.ConvertToDto() }, 200, ct);
        }
    }
}
