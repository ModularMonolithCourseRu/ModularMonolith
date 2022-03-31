namespace Shop.UseCases.Orders.Dtos;

public class OrderDetailsDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal Price { get; set; }

    public OrderDetailsItemDto[] Items { get; set; }
}