using DataModels;
using Shared.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Extensions
{
    public static class ProductExtensions
    {
        public static ProductDto ConvertModelToDto(this Product model)
        {
            return new ProductDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
            };
        }
        public static Product ConvertDtoToModel(this ProductDto dto)
        {
            return new Product
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
            };
        }
    }
}
