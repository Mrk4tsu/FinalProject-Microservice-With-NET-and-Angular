using FN.Application.Helper.Mail;
using FN.Application.System.Redis;
using FN.Utilities;
using FN.ViewModel.Helper;
using FN.ViewModel.Systems.User;
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
                switch ((string)channel)
                {
                    case SystemConstant.MESSAGE_REGISTER_EVENT:
                        var user = JsonSerializer.Deserialize<RegisterResponse>(message!);
                        if (user!.Status)
                            await _mailService.SendMail(user!.Email, $"Welcome! {user.FullName}", "Thank you for registering.", SystemConstant.TEMPLATE_ORDER_ID);
                        break;
                    case SystemConstant.MESSAGE_LOGIN_EVENT:
                        var userLogin = JsonSerializer.Deserialize<LoginResponse>(message!);
                        var emailContent = $@"
                            <h3>⚠️ Cảnh báo đăng nhập mới ⚠️</h3>
                            <p>Thời gian: {DateTime.UtcNow:dd/MM/yyyy HH:mm}</p>
                            <p>Thiết bị: {userLogin.DeviceInfo.DeviceType}</p>
                            <p>Hệ điều hành: {userLogin.DeviceInfo.OS}</p>
                            <p>Trình duyệt: {userLogin.DeviceInfo.Browser}</p>
                        ";
                        await _mailService.SendMail(userLogin!.Email, $"Welcome! {userLogin.Username}", emailContent);

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
