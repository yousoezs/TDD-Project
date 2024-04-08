using DataModels;
using Domain.Services.Repositories;
using Shared.Shared.DTOs;
using Shared.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.DataHandlers
{
    public static class OrderRepositoryDataHandler
    {
        #region Events

        public static event Func<Order, Task<ServiceResponse<Order>>> OnOrderCreated;
        public static event Func<Order, Task<ServiceResponse<Order>>> OnOrderUpdated;
        public static event Func<Guid, Task<ServiceResponse<Order>>> OnOrderDeleted;
        public static event Func<Guid, Task<ServiceResponse<Order>>> OnOrderFetched;
        public static event Func<Task<ServiceResponse<IEnumerable<Order>>>> OnAllOrderFetched;

        public static event ActionRef<Order> OnGetConvertedOrderDtoToModel;
        public static event ActionRef<OrderDto> OnGetConvertedOrderModelToDto;

        #region Invokes

        public static Task<ServiceResponse<Order>> OrderCreation(Order productDto) => OnOrderCreated?.Invoke(productDto);
        public static Task<ServiceResponse<Order>> OrderUpdate(Order productDto) => OnOrderUpdated?.Invoke(productDto);
        public static Task<ServiceResponse<Order>> OrderDeletion(Guid productId) => OnOrderDeleted?.Invoke(productId);
        public static Task<ServiceResponse<Order>> OrderFetched(Guid productId) => OnOrderFetched?.Invoke(productId);
        public static Task<ServiceResponse<IEnumerable<Order>>> FetchAllOrder() => OnAllOrderFetched?.Invoke();
        public static void GetConvertedOrderDtoToModel(ref Order order) => OnGetConvertedOrderDtoToModel?.Invoke(ref order);
        public static void GetConvertedOrderModelToDto(ref OrderDto orderDto) => OnGetConvertedOrderModelToDto?.Invoke(ref orderDto);

        #endregion

        #endregion
    }
}
