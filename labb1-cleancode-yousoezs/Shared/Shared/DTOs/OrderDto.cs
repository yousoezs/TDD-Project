using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Shared.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public List<ProductDto>? Products { get; set; } = new List<ProductDto>();
        public CustomerDto? Customer { get; set; }
        public DateTime ShippingDate { get; set; }
    }
}
