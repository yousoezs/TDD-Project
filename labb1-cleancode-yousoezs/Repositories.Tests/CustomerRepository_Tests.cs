using Shared.Shared.DTOs;
using FakeItEasy;
using DataAccess;
using Domain.Services.Repositories;
using Domain.Services.DataHandlers;
using Microsoft.EntityFrameworkCore;
using Shared.Shared.Interfaces;
using FluentAssertions;
using DataModels;
using Domain.Services.Extensions;
using Shared.Shared.Response;

namespace Repositories.Tests
{
    public class CustomerRepository_Tests
    {
        private static CustomerRepository_Tests _instance = new();

        private ICustomerRepository _customerRepository;

        #region InMemoryDb
        private static async Task<ShopContext> CreateShopContextWithInMemoryDbAsync()
        {
            var options = new DbContextOptionsBuilder<ShopContext>()
                .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
                .Options;

            var context = new ShopContext(options);

            context.Customers.AddRange(
                new Customer() { Id = Guid.NewGuid(), Name = "Albin", Password = "lösenord1" },
                new Customer() { Id = Guid.NewGuid(), Name = "Toni", Password = "lösenord2" }
            );

            await context.SaveChangesAsync();

            _instance._customerRepository = new CustomerRepository(context);

            return context;
        }
        #endregion

        [Fact]
        public async Task CustomerRepository_AddCustomer_ConvertDtoAndAddModelToDb()
        {
            // Arrange
            ShopContext context = await CreateShopContextWithInMemoryDbAsync();
            var fakeCustomer = A.Fake<CustomerDto>();
            fakeCustomer.Name = "Toni";
            fakeCustomer.Password = "Password";

            //Act
            var validFakeModel = await CustomerRepositoryDataHandler.CustomerCreation(fakeCustomer.ConvertToModel());
            if(!validFakeModel.Success) context.Customers.Should().HaveCount(2);
            context.SaveChanges();

            // Assert
            context.Customers.Should().HaveCount(3);
            context.Customers.Should().Contain(validFakeModel.Data);
        }

        /// <summary>
        /// Some explanation about this test and the naming.
        /// To see this test work correctly you first run the test normally and makes sure it passes.
        /// Then to see the actual test fail, you need to remove the name of the string. Meaning, the first InlineData("Geg)],
        /// leave a empty string there "" and the test will surely fail on that one calling the Assert.Fail and prompting why it fails.
        /// But the second run will work.
        /// </summary>
        /// <param name="name"></param>
        [Theory]
        [InlineData("Geg")]
        [InlineData("Tonko")]
        public async Task CustomerRepository_AddCustomer_ShouldFailFirst_ThenNot_Theory(string name)
        {
            // Arrange
            ShopContext context = await CreateShopContextWithInMemoryDbAsync();
            var fakeCustomer = A.Fake<CustomerDto>();

            fakeCustomer.Name = name;
            fakeCustomer.Password = "Password";

            // Act
            var validFakeModel = await CustomerRepositoryDataHandler.CustomerCreation(fakeCustomer.ConvertToModel());
            if (!validFakeModel.Success) Assert.Fail($"Something went wrong. Error Message: {validFakeModel.Message}");

            await context.SaveChangesAsync();
            // Assert
            context.Customers.Should().HaveCount(3);
            context.Customers.Should().Contain(validFakeModel.Data);
        }

        [Fact]
        public async Task CustomerRepository_FetchAllCustomers_ShouldReturnAllCustomersInDb()
        {
            //Arrange
            ShopContext context = await CreateShopContextWithInMemoryDbAsync();

            // Act
            var customerModels = await CustomerRepositoryDataHandler.FetchAllCustomer();

            // Assert
            context.Customers.Should().HaveCount(2);
            context.Customers.Should().Contain(customerModels.Data);
        }

        [Fact]
        public async Task CustomerRepository_GetCustomer_GetSingleCustomerFromDb_ById()
        {
            // Arrange
            ShopContext context = await CreateShopContextWithInMemoryDbAsync();
            ServiceResponse<Customer> retrievedCustomer = default;

            // Act
            retrievedCustomer = await CustomerRepositoryDataHandler.CustomerFetched(context.Customers.First().Id);
            if(!retrievedCustomer.Success) Assert.Fail("No Retrieved Customer Exists");

            // Assert
            context.Customers.Should().Contain(retrievedCustomer.Data);
        }

        [Fact]
        public async Task CustomerRepository_RemoveCustomer_RemoveCustomerFromDb()
        {
            // Arrange
            ShopContext context = await CreateShopContextWithInMemoryDbAsync();
            var fetchedCustomer = await CustomerRepositoryDataHandler.CustomerFetched(context.Customers.First().Id);

            // Act
            if(!fetchedCustomer.Success) Assert.Fail("No Retrieved Customer Exists");
            var removedCustomer = await CustomerRepositoryDataHandler.CustomerDeletion(fetchedCustomer.Data.Id);
            if(!removedCustomer.Success) Assert.Fail("We never deleted customer");
            await context.SaveChangesAsync();

            // Assert
            context.Customers.Should().HaveCount(1);
            Assert.Null(context.Customers.FirstOrDefault(c => c.Name.Equals("Albin")));
        }

        [Fact]
        public async Task CustomerRepository_UpdateCustomer_UpdateCustomerFromDbTable()
        {
            // Arrange
            ShopContext context = await CreateShopContextWithInMemoryDbAsync();
            var customer = await context.Customers.FirstAsync();
            customer.Name = "Geglash";
            customer.Password = "Gegosh";

            // Act
            var updatedCustomer = await CustomerRepositoryDataHandler.CustomerUpdate(customer);
            if(!updatedCustomer.Success) Assert.Fail("We never updated customer");
            await context.SaveChangesAsync();

            // Assert
            context.Customers.Should().HaveCount(2);
            context.Customers.Should().Contain(updatedCustomer.Data);
            context.Customers.Should().Contain(c => c.Name.Equals("Geglash"));
            context.Customers.Should().Contain(customer => customer.Password.Equals("Gegosh"));
        }
    }
}