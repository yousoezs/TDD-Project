using DataModels;
using Shared.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Extensions
{
    public static class CustomerExtensions
    {
        public static Customer ConvertToModel(this CustomerDto dto)
        {
            return new Customer
            {
                Id = dto.Id,
                Name = dto.Name,
                Password = dto.Password,
            };
        }

        public static CustomerDto ConvertToDto(this Customer model)
        {
            return new CustomerDto
            {
                Id = model.Id,
                Name = model.Name,
                Password = model.Password,
            };
        }
    }
}
