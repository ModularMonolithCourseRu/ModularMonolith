using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.UseCases.Orders.Commands.CreateOrder;
using Shop.UseCases.Orders.Dtos;
using Shop.UseCases.Orders.Queries.GetOrder;
using Shop.UseCases.Orders.Queries.GetOrders;

namespace Shop.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ISender _sender;

    public OrdersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<OrderListItemDto[]>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetOrdersQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDetailsDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetOrderQuery { Id = id }, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateOrderDto dto, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new CreateOrderCommand { Dto = dto }, cancellationToken);
        return Ok(result);
    }
}
