using Microsoft.EntityFrameworkCore;
using Shop.Entities.Models;

namespace Shop.Infrastructure.Interfaces.DataAccess;

public interface IDbContext
{
    DbSet<Order> Orders { get; }
    DbSet<Product> Products { get; }
    DbSet<Email> Emails { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    int SaveChanges();
}