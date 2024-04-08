using DataModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class ShopContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }

    public ShopContext(DbContextOptions<ShopContext> options) : base(options)
    {

    }
}