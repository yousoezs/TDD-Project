using DataModels;
using Shared.Shared.DTOs;
using Shared.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Shared.Interfaces
{
    public interface ICustomerRepository
    {
        Task<ServiceResponse<T>>AddCustomer<T>(T customer) where T : Customer;
        Task<ServiceResponse<T>> RemoveCustomer<T>(Guid customer) where T : Customer;
        Task<ServiceResponse<T>> UpdateCustomer<T>(T customer) where T : Customer;
        Task<ServiceResponse<T>> GetCustomer<T>(Guid customerID) where T : Customer;
        Task<ServiceResponse<IEnumerable<T>>> FetchAllCustomers<T>() where T : Customer;
    }
}
