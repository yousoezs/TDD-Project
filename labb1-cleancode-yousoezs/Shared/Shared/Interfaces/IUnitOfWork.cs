using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Shared.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public ICustomerRepository CustomerRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IProductRepository ProductRepository { get; }
        Task SaveAsync();
    }
}
