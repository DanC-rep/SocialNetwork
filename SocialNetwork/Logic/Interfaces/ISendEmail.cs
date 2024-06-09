namespace Logic.Interfaces
{
    public interface ISendEmail
    {
        Task SendEmailAsync(string toEmail, string subject, string body, bool isBodyHtml = false);
    }
}