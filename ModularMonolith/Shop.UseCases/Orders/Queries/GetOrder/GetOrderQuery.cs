using MediatR;
using Shop.UseCases.Orders.Dtos;

namespace Shop.UseCases.Orders.Queries.GetOrder;

public class GetOrderQuery : IRequest<OrderDetailsDto>
{
    public int Id { get; set; }
}