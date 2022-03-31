using Shop.Entities.Models;

namespace Shop.Infrastructure.Interfaces.DataAccess;

public interface IReadDbContext
{
    IQueryable<Order> Orders { get; }
    IQueryable<Product> Products { get; }
    IQueryable<Email> Emails { get; }
}