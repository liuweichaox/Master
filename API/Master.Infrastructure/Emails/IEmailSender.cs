namespace Master.Infrastructure.Emails
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailMessage message);
    }
}