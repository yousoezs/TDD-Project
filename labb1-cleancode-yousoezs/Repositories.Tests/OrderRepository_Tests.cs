using DataAccess;
using DataModels;
using Domain.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Shared.DTOs;
using Shared.Shared.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Domain.Services.DataHandlers;
using Domain.Services.Extensions;

namespace Repositories.Tests
{
    public class OrderRepository_Tests
    {
        private IOrderRepository _orderRepository;
        private static OrderRepository_Tests _instance = new ();

        #region InMemoryDb
        private static async Task<ShopContext> CreateShopContextWithInMemoryDbAsync()
        {
            var options = new DbContextOptionsBuilder<ShopContext>()
                .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
                .Options;

            var context = new ShopContext(options);

            context.Orders.AddRange(
                new Order { Id = Guid.NewGuid(), Customer = new Customer { Id = Guid.NewGuid(), Name = "Albin", Password = "lösenord1" }, Products = new List<Product> { new Product { Id = Guid.NewGuid(), Name = "Korv", Description = "Kryddstark korv" } } },
                new Order { Id = Guid.NewGuid(), Customer = new Customer { Id = Guid.NewGuid(), Name = "Toni", Password = "lösernord2"}, Products = new List<Product> { new Product { Id = Guid.NewGuid(), Name = "Bagel", Description = "En Bagel"} } });

            await context.SaveChangesAsync();
            
            _instance._orderRepository = new OrderRepository(context);

            return context;
        }
        #endregion

        [Fact]
        public async Task OrderRepository_AddOrder_AddOrderToDb()
        {
            // Arrange
            ShopContext context = await CreateShopContextWithInMemoryDbAsync();
            var order = A.Fake<OrderDto>();
            var customer = A.Fake<CustomerDto>();
            var product = A.Fake<ProductDto>();

            customer.Name = "Geglash";
            customer.Password = "Patata";

            product.Name = "Semla";
            product.Description = "En semla";

            order.Customer = customer;

            order.Products.Add(product);

            order.ShippingDate = DateTime.UtcNow;

            // Act
            var orderModel = await OrderRepositoryDataHandler.OrderCreation(order.ConvertToModel());
            if (!orderModel.Success) Assert.Fail($"Something went wrong: {orderModel.Message}");

            await context.SaveChangesAsync();

            // Assert
            context.Orders.Should().Contain(orderModel.Data);
            context.Orders.Should().HaveCount(3);
        }

        [Fact]
        public async Task OrderRepository_FetchAllOrders_ReturnsAllOrders()
        {
            // Arrange
            ShopContext context = await CreateShopContextWithInMemoryDbAsync();

            // Act
            var allOrder = await OrderRepositoryDataHandler.FetchAllOrder();

            // Assert
            context.Orders.Should().Contain(allOrder.Data);
            context.Orders.Should().HaveCount(2);
            context.Orders.Should().NotBeNull();
        }

        [Fact]
        public async void OrderRepository_GetOrder_ReturnsOneOrder()
        {
            // Arrange
            ShopContext context = await CreateShopContextWithInMemoryDbAsync();

            var order = await context.Orders.FirstAsync();

            // Act
            var validOrder = await OrderRepositoryDataHandler.OrderFetched(order.Id);
            if (!validOrder.Success) Assert.Fail($"Something went wrong: {validOrder.Message}");

            // Assert
            context.Orders.Should().Contain(validOrder.Data);
        }

        [Fact]
        public async void OrderRepository_RemoveOrder_RemovesOrderFromDbTable()
        {
            // Arrange
            ShopContext context = await CreateShopContextWithInMemoryDbAsync();
            var order = await context.Orders.FirstAsync();

            // Act
            var deletedOrder = await OrderRepositoryDataHandler.OrderDeletion(order.Id);

            if(!deletedOrder.Success) Assert.Fail($"Something went wrong: {deletedOrder.Message}");
            
            await context.SaveChangesAsync();

            // Assert
            context.Orders.Should().HaveCount(1);
            context.Orders.Should().NotContain(deletedOrder.Data);
        }

        [Fact]
        public async void OrderRepository_UpdateOrder_UpdatesCurrentOrderIn_DbTable()
        {
            // Arrange
            ShopContext context = await CreateShopContextWithInMemoryDbAsync();
            var order = await context.Orders.FirstAsync();
            order.Customer.Name = "UpdatedCustomer";
            order.Customer.Password = "UpdatedPassword";

            order.Products[0].Name = "UpdatedProduct";
            order.Products[0].Description = "UpdatedDescription";

            order.ShippingDate = DateTime.UtcNow;

            // Act
            var updatedOrder = await OrderRepositoryDataHandler.OrderUpdate(order);
            if(!updatedOrder.Success) Assert.Fail($"Something went wrong: {updatedOrder.Message}");
            await context.SaveChangesAsync();
            // Assert
            Assert.Equal(updatedOrder.Data, order);
        }
    }
}
