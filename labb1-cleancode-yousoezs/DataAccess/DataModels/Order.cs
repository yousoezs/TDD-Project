using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels;

public class Order
{
    public Guid Id { get; set; }
    public List<Product>? Products { get; set; } = new List<Product>();
    public Customer? Customer { get; set; }
    public DateTime ShippingDate { get; set; } = DateTime.UtcNow;
}