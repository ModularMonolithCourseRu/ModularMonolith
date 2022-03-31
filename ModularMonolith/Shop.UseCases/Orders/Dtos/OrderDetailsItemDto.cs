namespace Shop.UseCases.Orders.Dtos;

public class OrderDetailsItemDto
{
    public int ProductId { get; set; }
    public int Count { get; set; }
    public decimal ProductPrice { get; set; }
    public string ProductName { get; set; }
}