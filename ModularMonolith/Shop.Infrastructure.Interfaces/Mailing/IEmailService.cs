using Shop.Entities.Models;

namespace Shop.Infrastructure.Interfaces.Mailing;

public interface IEmailService
{
    public void SendEmail(EmailInfo email);
}