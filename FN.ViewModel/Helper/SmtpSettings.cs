namespace FN.ViewModel.Helper
{
    public class SmtpSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; }
    }
    public class MailSetting
    {
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
}
