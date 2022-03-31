using MediatR;
using Shop.UseCases.Emails.Dtos;

namespace Shop.UseCases.Emails.Queries.GetEmails;

public class GetEmailsQuery : IRequest<EmailDto[]>
{
    
}