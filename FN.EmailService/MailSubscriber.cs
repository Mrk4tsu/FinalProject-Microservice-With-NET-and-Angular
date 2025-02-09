using FN.Application.Helper.Mail;
using FN.Application.System.Redis;
using FN.Utilities;
using FN.ViewModel.Helper;
using FN.ViewModel.Systems.User;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text.Json;

namespace FN.EmailService
{
    public class MailSubscriber : BackgroundService
    {
        private readonly IMailService _mailService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRedisService _redisService;
        public MailSubscriber(IMailService mailService, IServiceProvider serviceProvider, IRedisService redisService)
        {
            _mailService = mailService;
            _serviceProvider = serviceProvider;
            _redisService = redisService;
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
                            {"ptime", $"{DateTime.UtcNow:dd/MM/yyyy HH:mm}"},
                            {"puser", userLogin.Username}
                        };
                        //var variable = new Dictionary<string, object>()
                        //{
                        //    {"pbrowser", userLogin!.DeviceInfo.Browser},
                        //    {"pos", userLogin.DeviceInfo.OS},
                        //    {"ptime", $"{DateTime.UtcNow:dd/MM/yyyy HH:mm}"},
                        //    {"puser", userLogin.Username}
                        //};
                        await _mailService.SendMail(userLogin!.Email, $"Cảnh báo bảo mật cho {userLogin.Username}", SystemConstant.TEMPLATE_WARNING_ID, variables);

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
    }
}
