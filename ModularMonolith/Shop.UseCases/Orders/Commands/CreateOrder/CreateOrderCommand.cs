using MediatR;
using Shop.UseCases.Orders.Dtos;

namespace Shop.UseCases.Orders.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<int>
{
    public CreateOrderDto Dto { get; set; }
}