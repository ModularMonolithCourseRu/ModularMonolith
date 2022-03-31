using Microsoft.EntityFrameworkCore;
using Shop.Entities.Models;
using Shop.Infrastructure.Interfaces.DataAccess;

namespace Shop.DataAccess.MsSql;

public class AppDbContext : DbContext, IDbContext, IReadDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Email> Emails { get; set; } = null!;

    IQueryable<Product> IReadDbContext.Products => Products.AsNoTracking();
    IQueryable<Email> IReadDbContext.Emails => Emails.AsNoTracking();
    IQueryable<Order> IReadDbContext.Orders => Orders.AsNoTracking();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Product 1",
                Price = 10
            },
            new Product
            {
                Id = 2,
                Name = "Product 2",
                Price = 100
            }
        );
    }
}