namespace Shop.Entities.Models;

public class Product : Aggregate
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}