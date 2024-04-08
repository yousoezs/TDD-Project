using DataAccess;
using DataModels;
using Domain.Services.DataHandlers;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Server.MinimalAPI.EndPoints.Order.Get.GetAll;
using FakeItEasy;
using Shared.Shared.Interfaces;
using Shared.Shared.Response;
using FluentAssertions;
using Shared.Shared.DTOs;
using Server.MinimalAPI.EndPoints.Product.Post;
using Domain.Services.Extensions;
using Server.MinimalAPI.EndPoints.Product.Get.GetAllProducts;

namespace Server.MinimalAPI.Tests;

public class EndpointsProducts_Tests
{
    private static EndpointsProducts_Tests _instance = new ();
    #region InMemoryDb
    private static async Task<ShopContext> CreateShopContextWithInMemoryDbAsync()
    {
        var options = new DbContextOptionsBuilder<ShopContext>()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options;

        var context = new ShopContext(options);

        context.Products.AddRange(
            new Product() { Id = Guid.NewGuid(), Name = "TestProduct", Description = "En Korv" },
            new Product() { Id = Guid.NewGuid(), Name = "Producto", Description = "En Bagel" }
        );

        await context.SaveChangesAsync();

        return context;
    }
    #endregion
    [Fact]
    public async Task AddRoutes_ShouldReturnAllProducts()
    {
        // Arrange
        ShopContext context = await CreateShopContextWithInMemoryDbAsync();
        var unitOfWork = A.Fake<IUnitOfWork>();

        var resp = new GetAllProductsResponse()
        {
            Products = new List<ProductDto>()
            {
                new ProductDto()
                {
                    Name = "TestProduct",
                    Description = "En Korv"
                },
                new ProductDto()
                {
                    Name = "Producto",
                    Description = "En Bagel"
                }
            }
        };

        var ep = Factory.Create<GetAllProductsEndpoint>(ctx => ctx.Request.RouteValues.Add("id", "1"), unitOfWork);

        ProductRepositoryDataHandler.OnProductsFetched += () => Task.FromResult(new ServiceResponse<IEnumerable<Product>>(true, resp.Products.Select(p => p.ConvertDtoToModel()), ""));
        var expectedStatus = 200;
        // Act

        var serviceResponse = await ProductRepositoryDataHandler.ProductsFetch();
        await ep.HandleAsync(default);

        //var resultAdd = epAdd.Response;
        var result = ep.Response;

        // Assert
        Assert.Equal(expectedStatus, ep.HttpContext.Response.StatusCode);
        context.Products.Should().HaveCount(2);
    }
}
