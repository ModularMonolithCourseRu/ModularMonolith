using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure.Interfaces.DataAccess;
using Shop.UseCases.Emails.Dtos;

namespace Shop.UseCases.Emails.Queries.GetEmails;

public class GetEmailsQueryHandler : IRequestHandler<GetEmailsQuery, EmailDto[]>
{
    private readonly IReadDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetEmailsQueryHandler(IReadDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public Task<EmailDto[]> Handle(GetEmailsQuery request, CancellationToken cancellationToken)
    {
        return _dbContext.Emails.ProjectTo<EmailDto>(_mapper.ConfigurationProvider).ToArrayAsync(cancellationToken);
    }
}