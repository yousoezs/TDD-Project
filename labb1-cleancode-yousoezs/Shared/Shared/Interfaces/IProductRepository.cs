using DataModels;
using Shared.Shared.DTOs;
using Shared.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Shared.Interfaces
{
    public interface IProductRepository
    {
        Task<ServiceResponse<T>> AddProudct<T>(T product) where T : Product;
        Task<ServiceResponse<T>> RemoveProduct<T>(Guid product) where T : Product;
        Task<ServiceResponse<T>> UpdateProduct<T>(T product) where T : Product;
        Task<ServiceResponse<T>>GetProduct<T>(Guid product) where T : Product;
        Task<ServiceResponse<IEnumerable<T>>> FetchAllProducts<T>() where T : Product;
    }
}
