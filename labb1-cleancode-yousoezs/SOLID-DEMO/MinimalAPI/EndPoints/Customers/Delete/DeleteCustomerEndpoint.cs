using Domain.Services.DataHandlers;
using Shared.Shared.Interfaces;
using FastEndpoints;
using Domain.Services.Extensions;

namespace Server.MinimalAPI.EndPoints.Customers.Delete
{
    public class DeleteCustomerEndpoint : Endpoint<DeleteCustomerRequest, DeleteCustomerResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCustomerEndpoint(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public override void Configure()
        {
            Delete("/delete/{id}");
            AllowAnonymous();
            Group<CustomerApiGroup>();
        }

        public override async Task HandleAsync(DeleteCustomerRequest request, CancellationToken ct)
        {
            if(!Guid.TryParse(request.Id, out var guid)) return;

            var response = await CustomerRepositoryDataHandler.CustomerDeletion(guid);

            if (!response.Success)
            {
                await SendAsync(new(), 400, ct);
                return;
            }

            await _unitOfWork.SaveAsync();
            await SendAsync(new(), 200, ct);
        }
    }
}
