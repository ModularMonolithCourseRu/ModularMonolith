namespace Shop.Entities.Models;

public class OrderItem : Entity
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Count { get; set; }

    public Product Product { get; set; }
}