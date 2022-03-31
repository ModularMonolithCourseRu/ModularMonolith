using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Interfaces.DataAccess;
using Shop.UseCases.Orders.Dtos;

namespace Shop.UseCases.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, OrderListItemDto[]>
{
    private readonly IReadDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetOrdersQueryHandler(IReadDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public Task<OrderListItemDto[]> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        return _dbContext.Orders
            .ProjectTo<OrderListItemDto>(_mapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken);
    }
}