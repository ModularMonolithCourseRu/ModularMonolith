namespace Shop.UseCases.Orders.Dtos;

public class CreateOrderDto
{
    public OrderItemDto[] Items { get; set; }
}