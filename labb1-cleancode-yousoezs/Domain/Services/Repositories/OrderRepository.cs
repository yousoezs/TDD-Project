using DataAccess;
using DataModels;
using Domain.Services.DataHandlers;
using Domain.Services.Extensions;
using Microsoft.EntityFrameworkCore;
using Shared.Shared.DTOs;
using Shared.Shared.Interfaces;
using Shared.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private ShopContext _dbContext;

        public OrderRepository(ShopContext dbContext)
        {
            _dbContext = dbContext;

            OrderRepositoryDataHandler.OnOrderCreated += AddOrder<Order>;
            OrderRepositoryDataHandler.OnOrderDeleted += RemoveOrder<Order>;
            OrderRepositoryDataHandler.OnOrderFetched += GetOrder<Order>;
            OrderRepositoryDataHandler.OnOrderUpdated += UpdateOrder<Order>;
            OrderRepositoryDataHandler.OnAllOrderFetched += FetchAllOrders<Order>;
        }

        public async Task<ServiceResponse<T>> AddOrder<T>(T item) where T : Order
        {
            item.Id = Guid.NewGuid();
            await _dbContext.AddAsync(item);
            return new ServiceResponse<T>(true, item, "Success");
        }

        public async Task<ServiceResponse<IEnumerable<T>>> FetchAllOrders<T>() where T : Order
        {
            var allOrder = await _dbContext.Orders.ToListAsync();
            return new ServiceResponse<IEnumerable<T>>(true, allOrder as IEnumerable<T>, "Success");
        }

        public async Task<ServiceResponse<T>> GetOrder<T>(Guid item) where T : Order
        {
            var validOrder = await _dbContext.Orders.FindAsync(item);
            if (validOrder is null)
                return new ServiceResponse<T>(false, null, "We failed to get order because valid order is null");

            return new ServiceResponse<T>(true, validOrder as T, "Success");
        }

        public async Task<ServiceResponse<T>> RemoveOrder<T>(Guid item) where T : Order
        {
            var validOrder = await _dbContext.Orders.FindAsync(item);
            if (validOrder is null)
                return new ServiceResponse<T>(false, null, "Failed to remove order because the found order was null");

            _dbContext.Orders.Remove(validOrder);
            return new ServiceResponse<T>(true, null, "Success");
        }

        public async Task<ServiceResponse<T>> UpdateOrder<T>(T item) where T : Order
        {
            var validOrder = await _dbContext.Orders.FindAsync(item.Id);
            if (validOrder is null)
                return new ServiceResponse<T>(false, null, "Failed to updated order because the found order was null");

            validOrder.Customer = item.Customer;
            validOrder.Products = item.Products;
            validOrder.ShippingDate = item.ShippingDate;
            
            _dbContext.Orders.Update(validOrder);
            return new ServiceResponse<T>(true, item, "Success");
        }
    }
}
