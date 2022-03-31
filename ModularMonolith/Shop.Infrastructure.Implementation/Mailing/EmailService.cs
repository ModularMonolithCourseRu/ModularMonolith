using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Shop.Infrastructure.Interfaces.Mailing;

namespace Shop.Infrastructure.Implementation.Mailing;

public class EmailService : IEmailService
{
    private readonly EmailOptions _options;
    private readonly SmtpClient _client;

    public EmailService(IOptions<EmailOptions> options)
    {
        _options = options.Value;

        _client = new SmtpClient(_options.Server, _options.Port);
        _client.Credentials = new NetworkCredential(_options.Email, _options.Password);
        _client.EnableSsl = true;
    }

    public void SendEmail(EmailInfo email)
    {
        var from = new MailAddress(_options.Email, _options.FromName);
        var to = new MailAddress(email.Address);
        var message = new MailMessage(from, to);
        message.Subject = email.Subject;
        message.Body = email.Body;

        _client.Send(message);
    }
}