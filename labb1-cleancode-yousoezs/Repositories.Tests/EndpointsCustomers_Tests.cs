using FakeItEasy;
using Shared.Shared.DTOs;
using Shared.Shared.Interfaces;
using Domain.Services.DataHandlers;
using Server.MinimalAPI.EndPoints.Customers.Post;
using FastEndpoints;
using Domain.Services.Extensions;
using Shared.Shared.Response;
using DataModels;
using Server.MinimalAPI.EndPoints.Customers.Put;

namespace Server.MinimalAPI.Tests
{
    public class EndpointsCustomers_Tests
    {
        [Fact]
        public async Task AddRoutes_ShouldAddCustomer_WhenCalledWithValidCustomerDto()
        {
            // Arrange
            var unitOfWork = A.Fake<IUnitOfWork>();
            var req = new AddCustomerRequest()
            {
                CustomerDto = new CustomerDto()
                {
                    Name = "Toni",
                    Password = "Pass"
                }
            };
           

            var ep = Factory.Create<AddCustomerEndpoint>(ctx => ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

            CustomerRepositoryDataHandler.OnCustomerCreated += (customer) => Task.FromResult(new ServiceResponse<Customer>(true, customer, ""));

            A.CallTo(() => unitOfWork.CustomerRepository.AddCustomer(req.CustomerDto.ConvertToModel()))
                .Returns(new ServiceResponse<Customer>(true, req.CustomerDto.ConvertToModel(), ""));

            var expectedStatus = 201;
            // Act
            var serviceResponse = await CustomerRepositoryDataHandler.CustomerCreation(req.CustomerDto.ConvertToModel());
            await ep.HandleAsync(req, default);
            var result = ep.Response;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.CustomerDto);
            Assert.Equal(expectedStatus, ep.HttpContext.Response.StatusCode);
        }
        [Fact]
        public async Task AddRoutes_ShouldNotAddCustomer_WhenCalledWithNoName()
        {
            // Arrange
            var unitOfWork = A.Fake<IUnitOfWork>();
            var req = new AddCustomerRequest()
            {
                CustomerDto = new CustomerDto()
                {
                    Name = "",
                    Password = ""
                }
            };


            var ep = Factory.Create<AddCustomerEndpoint>(ctx => ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

            CustomerRepositoryDataHandler.OnCustomerCreated += (customer) => Task.FromResult(new ServiceResponse<Customer>(false, null, ""));

            A.CallTo(() => unitOfWork.CustomerRepository.AddCustomer(req.CustomerDto.ConvertToModel()))
                .Returns(new ServiceResponse<Customer>(false, null, ""));

            var expectedStatus = 400;
            // Act
            var serviceResponse = await CustomerRepositoryDataHandler.CustomerCreation(req.CustomerDto.ConvertToModel());
            await ep.HandleAsync(req, default);
            var result = ep.Response;

            // Assert
            Assert.Null(result.CustomerDto);
            Assert.Equal(expectedStatus, ep.HttpContext.Response.StatusCode);
        }

        [Fact]
        public async Task AddRoutes_ShouldUpdateCustomer_WhenCalled()
        {
            // Arrange
            var unitOfWork = A.Fake<IUnitOfWork>();
            var req = new AddCustomerRequest()
            {
                CustomerDto = new CustomerDto()
                {
                    Name = "Toni",
                    Password = "Pass",
                }
            };
            var reqUpdate = new UpdateCustomerRequest()
            {
                CustomerDto = new CustomerDto()
                {
                    Name = "Toni",
                    Password = "Pass",
                }
            };

            var epUpdate = Factory.Create<UpdateCustomerEndpoint>(ctx => ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

            CustomerRepositoryDataHandler.OnCustomerCreated += (customer) => Task.FromResult(new ServiceResponse<Customer>(true, customer, ""));
            CustomerRepositoryDataHandler.OnCustomerUpdated += (customer) => Task.FromResult(new ServiceResponse<Customer>(true, customer, ""));
            A.CallTo(() => unitOfWork.CustomerRepository.AddCustomer(req.CustomerDto.ConvertToModel()))
                .Returns(new ServiceResponse<Customer>(true, req.CustomerDto.ConvertToModel(), ""));

            A.CallTo(() => unitOfWork.CustomerRepository.UpdateCustomer(req.CustomerDto.ConvertToModel()))
                .Returns(new ServiceResponse<Customer>(true, req.CustomerDto.ConvertToModel(), ""));

            var expectedStatus = 200;
            // Act
            var addedCustomerResponse = await CustomerRepositoryDataHandler.CustomerCreation(req.CustomerDto.ConvertToModel());
            reqUpdate.CustomerDto = addedCustomerResponse.Data.ConvertToDto();

            var updatedCustomerResponse = await CustomerRepositoryDataHandler.CustomerUpdate(reqUpdate.CustomerDto.ConvertToModel());
            await epUpdate.HandleAsync(reqUpdate, default);

            var result = epUpdate.Response;
            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.CustomerDto);
            Assert.Equal(expectedStatus, epUpdate.HttpContext.Response.StatusCode);
        }
    }
}
