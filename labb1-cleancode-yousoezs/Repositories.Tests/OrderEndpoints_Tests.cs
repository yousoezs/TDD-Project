
using DataModels;
using Domain.Services.DataHandlers;
using Domain.Services.Extensions;
using FakeItEasy;
using FastEndpoints;
using Server.MinimalAPI.EndPoints.Customers.Post;
using Server.MinimalAPI.EndPoints.Order.Post;
using Shared.Shared.DTOs;
using Shared.Shared.Interfaces;
using Shared.Shared.Response;

namespace Server.MinimalAPI.Tests;

public class OrderEndpoints_Tests
{
    [Fact]
    public async Task AddRoutes_ShouldAddOrderToDb()
    {
        // Arrange
        var unitOfWork = A.Fake<IUnitOfWork>();
        var req = new AddOrderRequest()
        {
            OrderDto = new OrderDto()
            {
                Customer = new CustomerDto()
                {
                    Name = "Gegosh",
                    Password = "Order"
                },
                Products = new List<ProductDto>()
                {
                    new ProductDto()
                    {
                        Name = "Korv",
                        Description = "En Korv"
                    },
                    new ProductDto()
                    {
                        Name = "Bagel",
                        Description = "En Bagel"
                    }
                },
                ShippingDate = DateTime.UtcNow,
            }
        };

        var ep = Factory.Create<AddOrderEndpoint>(ctx => ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        OrderRepositoryDataHandler.OnOrderCreated += (order) => Task.FromResult(new ServiceResponse<DataModels.Order>(true, order, ""));

        A.CallTo(() => unitOfWork.OrderRepository.AddOrder(req.OrderDto.ConvertToModel()))
            .Returns(new ServiceResponse<DataModels.Order>(true, req.OrderDto.ConvertToModel(), ""));

        var expectedStatus = 201;
        // Act
        var serviceResponse = await OrderRepositoryDataHandler.OrderCreation(req.OrderDto.ConvertToModel());
        await ep.HandleAsync(req, default);
        var result = ep.Response;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.OrderDto);
        Assert.Equal(expectedStatus, ep.HttpContext.Response.StatusCode);
    }
}
