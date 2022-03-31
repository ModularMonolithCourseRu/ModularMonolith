using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.UseCases.Emails.Dtos;
using Shop.UseCases.Emails.Queries.GetEmails;

namespace Shop.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailsController : ControllerBase
{
    private readonly ISender _sender;

    public EmailsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<EmailDto>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetEmailsQuery(), cancellationToken);
        return Ok(result);
    }
}