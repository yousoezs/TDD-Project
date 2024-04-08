using DataAccess;
using DataModels;
using Domain.Services.DataHandlers;
using Microsoft.EntityFrameworkCore;
using Shared.Shared.Interfaces;
using Shared.Shared.Response;

namespace Domain.Services.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private ShopContext _dbContext;

        public CustomerRepository(ShopContext dbContext) 
        {
            _dbContext = dbContext;

            CustomerRepositoryDataHandler.OnCustomerCreated += AddCustomer<Customer>;
            CustomerRepositoryDataHandler.OnCustomerDeleted += RemoveCustomer<Customer>;
            CustomerRepositoryDataHandler.OnCustomerUpdated += UpdateCustomer<Customer>;
            CustomerRepositoryDataHandler.OnCustomerFetched += GetCustomer<Customer>;
            CustomerRepositoryDataHandler.OnAllCustomerFetched += FetchAllCustomers<Customer>;
        }

        public async Task<ServiceResponse<T>> AddCustomer<T>(T customer) where T : Customer
        {
            customer.Id = Guid.NewGuid();
            await _dbContext.Customers.AddAsync(customer);

            if (customer.Name == string.Empty || customer.Name == "")
                return new ServiceResponse<T>(false, null, "There was no given name to the customer. You must enter a given name");

            return new ServiceResponse<T>(true, customer, "Success");
        }

        public async Task<ServiceResponse<IEnumerable<T>>> FetchAllCustomers<T>() where T : Customer
        {
            var customers = await _dbContext.Customers.ToListAsync();
            return new ServiceResponse<IEnumerable<T>>(true, customers as IEnumerable<T>, "Success");
        }

        public async Task<ServiceResponse<T>> GetCustomer<T>(Guid customerID) where T : Customer
        {
            var validCustomer = await _dbContext.Customers.FindAsync(customerID);
            if (validCustomer is null)
                return new ServiceResponse<T>(false, null, "Failed to fetch customer, either customer does not exist or id was wrong");

            return new ServiceResponse<T>(true, validCustomer as T, "Success");
        }

        public async Task<ServiceResponse<T>> RemoveCustomer<T>(Guid customer) where T : Customer
        {
            var validCustomer = await _dbContext.Customers.FindAsync(customer);
            if(validCustomer is null)
                return new ServiceResponse<T>(false, null, "Failed");

            _dbContext.Customers.Remove(validCustomer);
            return new ServiceResponse<T>(true, null, "Success");
        }

        public async Task<ServiceResponse<T>> UpdateCustomer<T>(T customer) where T : Customer
        {
            var validCustomer = await _dbContext.Customers.FindAsync(customer.Id);
            if (validCustomer is null)
                return new ServiceResponse<T>(false, null, "Failed");

            validCustomer.Name = customer.Name;
            validCustomer.Password = customer.Password;
            
            _dbContext.Customers.Update(validCustomer);
            return new ServiceResponse<T>(true, validCustomer as T, "Success");
        }
    }
}
