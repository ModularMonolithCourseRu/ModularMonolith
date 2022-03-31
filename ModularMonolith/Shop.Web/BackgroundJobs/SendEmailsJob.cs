using System.Linq;
using FluentScheduler;
using Shop.Infrastructure.Interfaces.DataAccess;
using Shop.Infrastructure.Interfaces.Mailing;

namespace Shop.Web.BackgroundJobs
{
    public class SendEmailsJob : IJob
    {
        private readonly IDbContext _dbContext;
        private readonly IEmailService _emailService;


        public SendEmailsJob(IDbContext dbContext, IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        public void Execute()
        {
            foreach (var email in _dbContext.Emails.Where(x => x.SentAt == null && x.Attempts < 3))
            {
                try
                {
                    _emailService.SendEmail(new EmailInfo { Address = email.Address, Subject = email.Subject, Body = email.Body });

                    email.SentAt = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    //logging

                    email.Attempts++;
                }
            }

            _dbContext.SaveChanges();
        }
    }
}
