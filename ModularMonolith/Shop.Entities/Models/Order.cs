namespace Shop.Entities.Models;

public class Order : Aggregate
{
    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }

    public ICollection<OrderItem> Items { get; set; }
}