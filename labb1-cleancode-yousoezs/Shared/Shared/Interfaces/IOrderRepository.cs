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
    public interface IOrderRepository
    {
        Task<ServiceResponse<T>> AddOrder<T>(T item) where T : Order;
        Task<ServiceResponse<T>> RemoveOrder<T>(Guid item) where T : Order;
        Task<ServiceResponse<T>> UpdateOrder<T>(T item) where T : Order;
        Task<ServiceResponse<IEnumerable<T>>> FetchAllOrders<T>() where T : Order;
        Task<ServiceResponse<T>> GetOrder<T>(Guid item) where T : Order;
    }
}
