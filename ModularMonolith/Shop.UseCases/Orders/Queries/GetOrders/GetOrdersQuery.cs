using MediatR;
using Shop.UseCases.Orders.Dtos;

namespace Shop.UseCases.Orders.Queries.GetOrders;
    
public class GetOrdersQuery : IRequest<OrderListItemDto[]>
{
    
}