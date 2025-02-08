using FN.Application.System.Redis;
using FN.Application.System.Token;
using FN.DataAccess.Entities;
using FN.Utilities;
using FN.ViewModel.Helper;
using FN.ViewModel.Systems.User;
using Microsoft.AspNetCore.Identity;

namespace FN.Application.System.User
{
    public class UserService : IUserService
    {
        private readonly IRedisService _redisService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        public UserService(IRedisService redisService,
            ITokenService tokenService,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _redisService = redisService;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<ApiResult<TokenResponse>> Authenticate(LoginDTO request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return new ApiErrorResult<TokenResponse>("Tài khoản không chính xác");

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded) return new ApiErrorResult<TokenResponse>("Tài khoản mật khẩu không chính xác");

            string clientId = request.ClientId;
            bool isNewDevice = false;

            if (string.IsNullOrEmpty(clientId)) clientId = Guid.NewGuid().ToString();
            if (!await _tokenService.IsDeviceRegistered(user.Id, clientId))
            {
                isNewDevice = true;
                await _tokenService.RegisterDevice(user.Id, clientId);
            }
            var deviceInfo = Commons.ParseUserAgent(request.UserAgent);
            if (isNewDevice)
            {
                var publish = new LoginResponse
                {
                    Email = user.Email!,
                    Username = user.UserName!,
                    DeviceInfo = deviceInfo
                };
                await _redisService.Publish(SystemConstant.MESSAGE_LOGIN_EVENT, publish);
            }
            var expires = request.RememberMe ? DateTime.Now.AddDays(14) : DateTime.Now.AddDays(3);
            var token = await _tokenService.GenerateAccessToken(user);
            var response = new TokenResponse
            {
                AccessToken = token,
                RefreshToken = _tokenService.GenerateRefreshToken(),
                RefreshTokenExpiry = expires,
                ClientId = clientId
            };
            await _tokenService.SaveRefreshToken(response.RefreshToken, user.Id, clientId, expires - DateTime.Now);
            return new ApiSuccessResult<TokenResponse>(response);
        }

        public async Task<ApiResult<bool>> Register(RegisterDTO request)
        {
            try
            {
                var cacheKey = $"user:{request.UserName}";
                var existed = await _redisService.KeyExist(cacheKey);

                if (existed || await _userManager.FindByNameAsync(request.UserName) != null)
                    return new ApiErrorResult<bool>("Tài khoản đã tồn tại");

                var user = new AppUser
                {
                    UserName = request.UserName,
                    FullName = request.FullName,
                    Email = request.Email,
                    Avatar = SystemConstant.AVATAR_DEFAULT,
                    TimeCreated = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                var response = new RegisterResponse
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Status = false
                };
                if (result.Succeeded)
                {
                    await _redisService.SetValue(cacheKey, user.UserName);
                    response.Status = true;
                    await _redisService.Publish(SystemConstant.MESSAGE_REGISTER_EVENT, response);
                    return new ApiSuccessResult<bool>();
                }
                await _redisService.Publish(SystemConstant.MESSAGE_REGISTER_EVENT, response);
                return new ApiErrorResult<bool>("Tạo mới tài khoản không thành công");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>(ex.Message);
            }
        }
    }
}
