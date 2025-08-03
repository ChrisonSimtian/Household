using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Household.Tests.Persistence.Fixtures;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}

public class User
{
    [Key]
    public string Id { get; set; }
    public string Name { get; set; }
}
public class Order
{
    [Key]
    public string Id { get; set; }
    public string UserId { get; set; }
    public DateTime OrderDate { get; set; }
}
public class OrderItem
{
    [Key]
    public string Id { get; set; }
    public string OrderId { get; set; }
    public string ProductId { get; set; }
    public int Quantity { get; set; }
}
public class Product
{
    [Key]
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}