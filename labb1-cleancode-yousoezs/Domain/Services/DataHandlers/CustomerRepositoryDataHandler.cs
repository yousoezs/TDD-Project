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
    public static class CustomerRepositoryDataHandler
    {
        #region Events

        public static event Func<Customer, Task<ServiceResponse<Customer>>> OnCustomerCreated;
        public static event Func<Customer, Task<ServiceResponse<Customer>>> OnCustomerUpdated;
        public static event Func<Guid, Task<ServiceResponse<Customer>>> OnCustomerDeleted;
        public static event Func<Guid, Task<ServiceResponse<Customer>>> OnCustomerFetched;
        public static event Func<Task<ServiceResponse<IEnumerable<Customer>>>> OnAllCustomerFetched;

        public static event ActionRef<CustomerDto> OnGetCustomerDto;
        public static event ActionRef<Customer> OnGetCustomerModel;

        #region Invokes

        public static Task<ServiceResponse<Customer>> CustomerCreation(Customer productDto) => OnCustomerCreated?.Invoke(productDto);
        public static Task<ServiceResponse<Customer>> CustomerUpdate(Customer productDto) => OnCustomerUpdated?.Invoke(productDto);
        public static Task<ServiceResponse<Customer>> CustomerDeletion(Guid productId) => OnCustomerDeleted?.Invoke(productId);
        public static Task<ServiceResponse<Customer>> CustomerFetched(Guid productId) => OnCustomerFetched?.Invoke(productId);
        public static Task<ServiceResponse<IEnumerable<Customer>>> FetchAllCustomer() => OnAllCustomerFetched?.Invoke();
        public static void GetConvertedCustomerModelToDto(ref CustomerDto customerDto) => OnGetCustomerDto?.Invoke(ref customerDto);
        public static void GetConvertedCustomerDtoToModel(ref Customer customer) => OnGetCustomerModel?.Invoke(ref customer);
        #endregion

        #endregion
    }
}
