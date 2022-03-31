using AutoMapper;
using MediatR;
using Shop.Entities.Models;
using Shop.Infrastructure.Interfaces.DataAccess;
using Shop.Infrastructure.Interfaces.Web;

namespace Shop.UseCases.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly IDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CreateOrderCommandHandler(IDbContext dbContext, IMapper mapper, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = _mapper.Map<Order>(request.Dto);
        order.CreatedAt = DateTime.UtcNow;
        order.CreatedBy = _currentUserService.Id;
        _dbContext.Orders.Add(order);

        var newMail = new Email
        {
            Address = _currentUserService.Email,
            Subject = "Order created",
            Body = "Your order created successfully",
            UserId = _currentUserService.Id,
            Order = order,
            CreatedAt = DateTime.UtcNow
        };
        _dbContext.Emails.Add(newMail);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}