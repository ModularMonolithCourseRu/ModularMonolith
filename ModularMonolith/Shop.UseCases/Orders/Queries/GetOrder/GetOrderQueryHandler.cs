using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Interfaces.DataAccess;
using Shop.UseCases.Exceptions;
using Shop.UseCases.Orders.Dtos;

namespace Shop.UseCases.Orders.Queries.GetOrder;

public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderDetailsDto>
{
    private readonly IReadDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetOrderQueryHandler(IReadDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<OrderDetailsDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Orders
            .ProjectTo<OrderDetailsDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (result == null) throw new EntityNotFoundException();

        return result;
    }
}