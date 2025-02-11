using FN.Application.Helper.Mail;
using FN.Application.Systems.Redis;
using FN.Utilities;
using FN.ViewModel.Helper;
using FN.ViewModel.Systems.User;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net;
using System.Text.Json;

namespace FN.EmailService
{
    public class MailSubscriber : BackgroundService
    {
        private readonly IMailService _mailService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRedisService _redisService;
        private readonly IConfiguration _configuration;
        public MailSubscriber(IMailService mailService, IServiceProvider serviceProvider, IRedisService redisService, IConfiguration configuration)
        {
            _mailService = mailService;
            _serviceProvider = serviceProvider;
            _redisService = redisService;
            _configuration = configuration;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _redisService.Subscribe(SystemConstant.MESSAGE_PATTERN_EVENT, async (channel, message) =>
            {
                using var scope = _serviceProvider.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IMailService>();
                switch ((string)channel!)
                {
                    case SystemConstant.MESSAGE_REGISTER_EVENT:
                        var user = JsonSerializer.Deserialize<RegisterResponse>(message!);
                        var vars = new Dictionary<string, object>()
                        {
                            {"pusername", user?.FullName!}
                        };
                        if (user!.Status)
                            await _mailService.SendMail(user!.Email, $"Chào mừng {user.FullName} đến MrKatsu Shop!", SystemConstant.TEMPLATE_WELCOME_ID, vars);
                        break;
                    case SystemConstant.MESSAGE_LOGIN_EVENT:
                        var userLogin = JsonSerializer.Deserialize<LoginResponse>(message!);
                        var variables = new JObject
                        {
                            {"pbrowser", userLogin!.DeviceInfo.Browser},
                            {"pos", userLogin.DeviceInfo.OS},
                            {"ptime", userLogin.DeviceInfo.LastLogin.ToString("hh:mm dd/MM/yyyy")},
                            {"puser", userLogin.Username},
                            {"pip", userLogin.DeviceInfo.IPAddress}
                        };
                        await _mailService.SendMail(userLogin!.Email, $"Cảnh báo bảo mật cho {userLogin.Username}", SystemConstant.TEMPLATE_WARNING_ID, variables);
                        break;
                    case SystemConstant.MESSAGE_UPDATE_EMAIL_EVENT:
                        var req = JsonSerializer.Deserialize<UpdateEmailResponse>(message!);
                        var baseDomain = _configuration["BaseDomain"];
                        var link = UrlCallback(req.UserId, req.NewEmail, req.Token, baseDomain ?? "https://mrkatsu.io.vn");
                        var obj = new JObject
                        {
                            {"plink", link}
                        };
                        await _mailService.SendMail(req!.NewEmail, $"Xác nhận thay đổi email", SystemConstant.TEMPLATE_UPDATE_MAIL_ID, obj);
                        break;
                    default:
                        break;
                }

            });
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        private string UrlCallback(int userId, string newEmail, string token, string domain)
        {
            var encodedToken = WebUtility.UrlEncode(token);
            return $"{domain}/confirm-email?userId={userId}&newEmail={newEmail}&token={encodedToken}";
        }
    }
}
