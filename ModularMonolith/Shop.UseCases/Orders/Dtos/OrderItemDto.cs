using System.ComponentModel.DataAnnotations;

namespace Shop.UseCases.Orders.Dtos;

public class OrderItemDto
{
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Count { get; set; }
}