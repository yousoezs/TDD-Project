using DataAccess;
using DataModels;
using Domain.Services.DataHandlers;
using Domain.Services.Extensions;
using Domain.Services.Repositories;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shared.Shared.DTOs;
using Shared.Shared.Interfaces;
using Shared.Shared.Response;

namespace Repositories.Tests
{
    public class ProductRepository_Tests
    {
        private IProductRepository _productRepository;
        private static ProductRepository_Tests _instance = new ();

        #region InMemoryDb
        private static async Task<ShopContext> CreateShopContextWithInMemoryDbAsync()
        {
            var options = new DbContextOptionsBuilder<ShopContext>()
                .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
                .Options;

            var context = new ShopContext(options);

            context.Products.AddRange(
                new Product() { Id = Guid.NewGuid(), Name = "Korv", Description = "En Korv" },
                new Product() { Id = Guid.NewGuid(), Name = "Bagel", Description = "En Bagel" }
            );

            await context.SaveChangesAsync();

            _instance._productRepository = new ProductRepository(context);

            return context;
        }
        #endregion

        [Fact]
        public async Task ProductRepository_AddProduct_ConvertDtoAndAddModelToDb()
        {
            // Arrange
            ShopContext context = await CreateShopContextWithInMemoryDbAsync();
            var productDto = A.Fake<ProductDto>();
            productDto.Name = "Snickers";
            productDto.Description = "Are you hungry? Eat a snickers!";

            // Act
            var productModel = await ProductRepositoryDataHandler.ProductCreation(productDto.ConvertDtoToModel());
            if (!productModel.Success) Assert.Fail($"Something went wrong: {productModel.Message}");
            await context.SaveChangesAsync();
            // Assert
            context.Products.Should().Contain(productModel.Data);
            context.Products.Should().HaveCount(3);
        }

        [Fact]
        public async Task ProductRepository_FetchAllProducts_ShouldReturnAllProductsInDb()
        {
            // Arrange
            ShopContext context = await CreateShopContextWithInMemoryDbAsync();

            // Act
            var productModels = await ProductRepositoryDataHandler.ProductsFetch();

            // Assert
            context.Products.Should().HaveCount(2);
            context.Products.Should().Contain(productModels.Data);
        }

        [Fact]
        public async void ProductRepository_GetProduct_GetSingleProductFromDbById()
        {
            // Arrange
            ShopContext context = await CreateShopContextWithInMemoryDbAsync();
            ServiceResponse<Product> retrievedProduct = default; 

            // Act
            retrievedProduct = await ProductRepositoryDataHandler.ProductFetched(context.Products.First().Id);
            if(!retrievedProduct.Success) Assert.Fail($"Something went wrong: {retrievedProduct.Message}");

            // Assert
            context.Products.Should().Contain(context.Products.ToList());
        }

        [Fact]
        public async Task ProductRepository_RemoveProduct_RemoveProductFromDb()
        {
            // Arrange
            ShopContext context = await CreateShopContextWithInMemoryDbAsync();
            var fetchedProduct = await ProductRepositoryDataHandler.ProductFetched(context.Products.First().Id);

            // Act
            if(!fetchedProduct.Success) Assert.Fail($"Something went wrong: {fetchedProduct.Message}");
            var removedProduct = await ProductRepositoryDataHandler.ProductDelete(fetchedProduct.Data.Id);
            if(!removedProduct.Success) Assert.Fail($"Something went wrong: {removedProduct.Message}");
            await context.SaveChangesAsync();
            // Assert
            context.Products.Should().HaveCount(1);
            Assert.Null(context.Products.FirstOrDefault(c => c.Name.Equals("Korv")));
        }

        [Fact]
        public async Task ProductRepository_UpdateProduct_UpdateProductFromDbTable()
        {
            // Arrange
            ShopContext context = await CreateShopContextWithInMemoryDbAsync();
            var retrievedProduct = await context.Products.FirstAsync();

            retrievedProduct.Name = "Rasputinos";
            retrievedProduct.Description = "Gegen";

            // Act
            var updatedProduct = await ProductRepositoryDataHandler.ProductUpdate(retrievedProduct);

            await context.SaveChangesAsync();

            // Assert
            context.Products.Should().HaveCount(2);
            context.Products.Should().Contain(updatedProduct.Data);
            context.Products.Should().Contain(c => c.Name.Equals("Rasputinos"));
            context.Products.Should().Contain(product => product.Description.Equals("Gegen"));
            Assert.Equal("Rasputinos", updatedProduct.Data.Name);

        }
    }
}
