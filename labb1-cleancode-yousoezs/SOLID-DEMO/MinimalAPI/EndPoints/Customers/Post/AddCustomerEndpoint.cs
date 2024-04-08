using Domain.Services.DataHandlers;
using Domain.Services.Extensions;
using FastEndpoints;
using Shared.Shared.Interfaces;

namespace Server.MinimalAPI.EndPoints.Customers.Post
{
    public class AddCustomerEndpoint : Endpoint<AddCustomerRequest, AddCustomerResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddCustomerEndpoint(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public override void Configure()
        {
            Post("/add");
            AllowAnonymous();
            Group<CustomerApiGroup>();
        }

        public override async Task HandleAsync(AddCustomerRequest request, CancellationToken ct)
        {
            var response = await CustomerRepositoryDataHandler.CustomerCreation(request.CustomerDto.ConvertToModel());

            if (!response.Success)
            {
                await SendAsync(new(), 400, ct);
                return;
            }

            await _unitOfWork.SaveAsync();
            await SendAsync(new() { CustomerDto = response.Data.ConvertToDto() }, 201, ct);
        }
    }
}
