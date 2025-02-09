using FN.ViewModel.Helper;
using Mailjet.Client.Resources;
using Mailjet.Client;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json.Linq;
using FN.Utilities;

namespace FN.Application.Helper.Mail
{
    public class MailService : IMailService
    {
        private readonly MailSetting _smtpSettings;
        public MailService(IOptions<MailSetting> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }
        public async Task<bool> SendMail(string toMail, string subject, string templateId, Dictionary<string, object> variables)
        {
            MailjetClient client = new MailjetClient(_smtpSettings.ApiKey, _smtpSettings.SecretKey);

            // Chuyển đổi Dictionary thành JObject
            JObject variablesObject = new JObject();
            foreach (var item in variables)
            {
                variablesObject[item.Key] = JToken.FromObject(item.Value);
            }

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property("Variables", variablesObject)
            .Property(Send.FromEmail, _smtpSettings.SenderEmail)
            .Property(Send.FromName, _smtpSettings.SenderName)
            .Property(Send.Subject, subject)
            .Property(Send.MjTemplateID, templateId)
            .Property(Send.MjTemplateLanguage, true)
            .Property(Send.To, toMail);
            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(string.Format("Total: {0}, Count: {1}", response.GetTotal(), response.GetCount()));
                Console.WriteLine(response.GetData());
                return true;
            }
            else
            {
                Console.WriteLine(string.Format("StatusCode: {0}", response.StatusCode));

                Console.WriteLine(string.Format("ErrorInfo: {0}", response.GetErrorInfo()));

                Console.WriteLine(response.GetData());
                Console.WriteLine(string.Format("ErrorMessage: {0}", response.GetErrorMessage()));
                return false;
            }
        }

        public async Task<bool> SendMail(string toEmail, string subject, string body)
        {
            MailjetClient client = new MailjetClient(_smtpSettings.ApiKey, _smtpSettings.SecretKey);
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }.Property(Send.FromEmail, _smtpSettings.SenderEmail)
            .Property(Send.FromName, _smtpSettings.SenderName)
            .Property(Send.Subject, subject)
            .Property(Send.TextPart, body)
            .Property(Send.To, toEmail);
            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(string.Format("Total: {0}, Count: {1}", response.GetTotal(), response.GetCount()));
                Console.WriteLine(response.GetData());
                return true;
            }
            else
            {
                Console.WriteLine(string.Format("StatusCode: {0}", response.StatusCode));

                Console.WriteLine(string.Format("ErrorInfo: {0}", response.GetErrorInfo()));

                Console.WriteLine(response.GetData());
                Console.WriteLine(string.Format("ErrorMessage: {0}", response.GetErrorMessage()));
                return false;
            }
        }
    }
}
