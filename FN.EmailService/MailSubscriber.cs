using FN.Application.Helper.Mail;
using FN.Application.System.Redis;
using FN.Utilities;
using FN.ViewModel.Systems.User;
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
            await _redisService.Subscribe(SystemConstant.MESSAGE_REGISTER_EVENT, async (channel, message) =>
            {
                using var scope = _serviceProvider.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IMailService>();

                var user = JsonSerializer.Deserialize<RegisterResponse>(message!);
                if (user!.Status)
                    await _mailService.SendMail(user!.Email, $"Welcome! {user.FullName}", "Thank you for registering.", SystemConstant.TEMPLATE_ORDER_ID);
            });
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
