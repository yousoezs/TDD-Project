using DataAccess;
using DataModels;
using Domain.Services.DataHandlers;
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
    public class ProductRepository : IProductRepository
    {
        private ShopContext _dbContext;

        public ProductRepository(ShopContext dbContext)
        {
            _dbContext = dbContext;

            ProductRepositoryDataHandler.OnProductCreated += AddProudct<Product>;
            ProductRepositoryDataHandler.OnProductDeleted += RemoveProduct<Product>;
            ProductRepositoryDataHandler.OnProductUpdated += UpdateProduct<Product>;
            ProductRepositoryDataHandler.OnProductFetched += GetProduct<Product>;
            ProductRepositoryDataHandler.OnProductsFetched += FetchAllProducts<Product>;
        }

        public async Task<ServiceResponse<T>> AddProudct<T>(T product) where T : Product
        {
            product.Id = Guid.NewGuid();
            await _dbContext.Products.AddAsync(product);

            if(product.Name == "" || product.Name == string.Empty)
                return new ServiceResponse<T>(false, null, "No given name was set to the product. Please enter a given name");

            return new ServiceResponse<T>(true, product, "Success");
        }

        public async Task<ServiceResponse<IEnumerable<T>>> FetchAllProducts<T>() where T : Product
        {
            var allProducts = await _dbContext.Products.ToListAsync();
            return new ServiceResponse<IEnumerable<T>>(true, allProducts as IEnumerable<T>, "Success");
        }

        public async Task<ServiceResponse<T>> GetProduct<T>(Guid product) where T : Product
        {
            var validProduct = await _dbContext.Products.FindAsync(product);
            if (validProduct is null)
                return new ServiceResponse<T>(false, null, "Failed to fetch product, either product does not exist or id was wrong.");

            return new ServiceResponse<T>(true, validProduct as T, "Success");
        }

        public async Task<ServiceResponse<T>> RemoveProduct<T>(Guid product) where T : Product
        {
            var validProduct = await _dbContext.Products.FindAsync(product);
            if (validProduct is null)
                return new ServiceResponse<T>(false, null, "Failed");

            _dbContext.Products.Remove(validProduct);
            return new ServiceResponse<T>(true, null, "Success");
        }

        public async Task<ServiceResponse<T>> UpdateProduct<T>(T product) where T : Product
        {
            var validProduct = await _dbContext.Products.FindAsync(product.Id);
            if (validProduct is null)
                return new ServiceResponse<T>(false, null, "Failed to update product because product is null");

            validProduct.Name = product.Name;
            validProduct.Description = product.Description;

            _dbContext.Products.Update(validProduct);
            return new ServiceResponse<T>(true, validProduct as T, "Success");
        }
    }
}
