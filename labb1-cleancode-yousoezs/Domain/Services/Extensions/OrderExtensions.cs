using DataModels;
using Shared.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Extensions
{
    public static class OrderExtensions
    {
        public static OrderDto ConvertToDto(this Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                Customer = order.Customer.ConvertToDto(),
                Products = order.Products.Select(x => x.ConvertModelToDto()).ToList(),
                ShippingDate = order.ShippingDate
            };
        }

        public static Order ConvertToModel(this OrderDto order)
        {
            return new Order
            {
                Id = order.Id,
                Customer = order.Customer.ConvertToModel(),
                Products = order.Products.Select(x => x.ConvertDtoToModel()).ToList(),
                ShippingDate = order.ShippingDate,
            };
        }
    }
}
