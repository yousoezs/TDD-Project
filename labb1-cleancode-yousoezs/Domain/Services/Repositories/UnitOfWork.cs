using DataAccess;
using Shared.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShopContext _dbContext;

        public ICustomerRepository CustomerRepository { get; }

        public IOrderRepository OrderRepository { get; }

        public IProductRepository ProductRepository { get; }

        public UnitOfWork(ShopContext context)
        {
            _dbContext = context;

            CustomerRepository = new CustomerRepository(_dbContext);
            OrderRepository = new OrderRepository(_dbContext);
            ProductRepository = new ProductRepository(_dbContext);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public void Dispose()
        {
           _dbContext.Dispose();
        }
    }
}
