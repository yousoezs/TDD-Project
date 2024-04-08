using DataModels;
using Domain.Services.Repositories;
using Shared.Shared.DTOs;
using Shared.Shared.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.DataHandlers;

public static class ProductRepositoryDataHandler
{
    #region Events

    public static event Func<Product, Task<ServiceResponse<Product>>> OnProductCreated;
    public static event Func<Product, Task<ServiceResponse<Product>>> OnProductUpdated;
    public static event Func<Guid, Task<ServiceResponse<Product>>> OnProductDeleted;
    public static event Func<Guid, Task<ServiceResponse<Product>>> OnProductFetched;
    public static event Func<Task<ServiceResponse<IEnumerable<Product>>>> OnProductsFetched;

    public static event ActionRef<ProductDto> OnGetProductDto;
    public static event ActionRef<Product> OnGetProductModel;

    #region Invokes

    public static Task<ServiceResponse<Product>> ProductCreation(Product productDto) => OnProductCreated?.Invoke(productDto);
    public static Task<ServiceResponse<Product>> ProductUpdate(Product productDto) => OnProductUpdated?.Invoke(productDto);
    public static Task<ServiceResponse<Product>> ProductDelete(Guid productId) => OnProductDeleted?.Invoke(productId);
    public static Task<ServiceResponse<Product>> ProductFetched(Guid productId) => OnProductFetched?.Invoke(productId);
    public static Task<ServiceResponse<IEnumerable<Product>>>ProductsFetch() => OnProductsFetched?.Invoke();
    public static void GetConvertedProductModelToDto(ref ProductDto productDto) => OnGetProductDto?.Invoke(ref productDto);
    public static void GetConvertedProductDtoToModel(ref Product product) => OnGetProductModel?.Invoke(ref product);
    #endregion

    #endregion
}
