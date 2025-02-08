namespace FN.Application.Helper.Mail
{
    public interface IMailService
    {
        Task<bool> SendMail<T>(string toMail, string subject, string templateId, Dictionary<string, object> variables);
        Task<bool> SendMail(string toEmail, string subject, string body, string templateId);
        Task<bool> SendMail(string toEmail, string subject, string body);
    }
}
