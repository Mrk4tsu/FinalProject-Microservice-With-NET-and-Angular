using AutoMapper;
using CloudinaryDotNet;
using FN.Application.Helper.Images;
using FN.Application.Systems.Redis;
using FN.Application.Systems.Token;
using FN.DataAccess.Entities;
using FN.Utilities;
using FN.ViewModel.Helper.API;
using FN.ViewModel.Helper.Device;
using FN.ViewModel.Systems.Token;
using FN.ViewModel.Systems.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net;

namespace FN.Application.Systems.User
{
    public class UserService : IUserService
    {
        private readonly IRedisService _redisService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IMongoCollection<UserDevice> _userDevicesCollection;
        public UserService(IRedisService redisService,
                        IMongoDatabase database,
                        ITokenService tokenService,
                        IMapper mapper,
                        IImageService imageService,
                        UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _redisService = redisService;
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _imageService = imageService;
            _userDevicesCollection = database.GetCollection<UserDevice>("UserDevices");
        }
        private string GetClientIP(HttpContext context)
        {
            var ipHeaders = new[] { "X-Forwarded-For", "Forwarded", "X-Real-IP" };
            foreach (var header in ipHeaders)
            {
                if (context.Request.Headers.TryGetValue(header, out var headerValue))
                {
                    var ip = headerValue.ToString().Split(',')[0].Trim();
                    if (!string.IsNullOrEmpty(ip) && IsIPv4(ip))
                        return ip;
                }
            }

            var remoteIp = context.Connection.RemoteIpAddress;
            if (remoteIp != null)
            {
                if (remoteIp.Equals(IPAddress.IPv6Loopback))
                    return "127.0.0.1";

                if (remoteIp.IsIPv4MappedToIPv6)
                    return remoteIp.MapToIPv4().ToString();

                return remoteIp.ToString();
            }

            return "Unknown";
        }
        private bool IsIPv4(string ip)
        {
            return IPAddress.TryParse(ip, out var address) &&
                   address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork;
        }
        public async Task<ApiResult<TokenResponse>> RefreshToken(RefreshTokenRequest request)
        {
            var currentToken = await _tokenService.GetRefreshToken(request);
            if (currentToken == null || currentToken != request.RefreshToken)
                return new ApiErrorResult<TokenResponse>("Refresh token không hợp lệ");
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null) return new ApiErrorResult<TokenResponse>("Tài khoản không tồn tại");

            var token = await _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var response = new TokenResponse
            {
                AccessToken = token,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiry = DateTime.Now.AddDays(3),
                ClientId = request.ClientId
            };
            await _tokenService.SaveRefreshToken(newRefreshToken, request, response.RefreshTokenExpiry - DateTime.Now);
            return new ApiSuccessResult<TokenResponse>(response);
        }
        public async Task<ApiResult<TokenResponse>> Authenticate(LoginDTO request, HttpContext context)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return new ApiErrorResult<TokenResponse>("Tài khoản không chính xác");

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded) return new ApiErrorResult<TokenResponse>("Tài khoản mật khẩu không chính xác");

            string clientId = request.ClientId;
            var ipAddress = GetClientIP(context);
            bool isNewDevice = false;
            if (string.IsNullOrEmpty(clientId)) clientId = Guid.NewGuid().ToString();
            var tokenReq = new TokenRequest
            {
                UserId = user.Id,
                ClientId = clientId
            };
            if (!await IsDeviceRegistered(tokenReq))
            {
                isNewDevice = true;
                await SaveDeviceInfo(tokenReq, request.UserAgent, ipAddress);
            }
            var deviceInfo = Commons.ParseUserAgent(request.UserAgent);
            var device = new DeviceInfoDetail
            {
                ClientId = clientId,
                Browser = deviceInfo.Browser,
                DeviceType = deviceInfo.DeviceType,
                IPAddress = ipAddress,
                LastLogin = DateTime.Now,
                OS = deviceInfo.OS
            };
            if (isNewDevice)
            {
                var publish = new LoginResponse
                {
                    Email = user.Email!,
                    Username = user.UserName!,
                    DeviceInfo = device
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
            await _tokenService.SaveRefreshToken(response.RefreshToken, tokenReq, expires - DateTime.Now);
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
        public async Task<ApiResult<bool>> RevokeDevice(TokenRequest request)
        {
            try
            {
                //1. Xóa Client khỏi danh sách thiết bị đã đăng ký
                var filter = Builders<UserDevice>.Filter.Eq(u => u.UserId, request.UserId);
                var update = Builders<UserDevice>.Update.PullFilter(u => u.Devices, d => d.ClientId == request.ClientId);

                await _userDevicesCollection.UpdateOneAsync(filter, update);
                //2. Xóa Refresh Token
                await _tokenService.RemoveRefreshToken(request);
                return new ApiSuccessResult<bool>();
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>(ex.Message);
            }
        }
        public async Task SaveDeviceInfo(TokenRequest request, string userAgent, string ipAddress)
        {
            var device = Commons.ParseUserAgent(userAgent);
            var deviceInfo = new DeviceInfoDetail
            {
                ClientId = request.ClientId,
                Browser = device.Browser,
                DeviceType = device.DeviceType,
                IPAddress = ipAddress,
                LastLogin = DateTime.Now,
                OS = device.OS,
            };
            var filter = Builders<UserDevice>.Filter.Eq(u => u.UserId, request.UserId);
            var update = Builders<UserDevice>.Update.Push(u => u.Devices, deviceInfo);
            await _userDevicesCollection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }
        public async Task<ApiResult<List<DeviceInfoDetail>>> GetRegisteredDevices(int userId)
        {
            var userDevice = await _userDevicesCollection.Find(u => u.UserId == userId).FirstOrDefaultAsync();
            if (userDevice == null)
            {
                return new ApiErrorResult<List<DeviceInfoDetail>>("Không có CLJ hết");
            }
            return new ApiSuccessResult<List<DeviceInfoDetail>>(userDevice.Devices);
        }
        public async Task<bool> IsDeviceRegistered(TokenRequest request)
        {
            var filter = Builders<UserDevice>.Filter.And(
            Builders<UserDevice>.Filter.Eq(u => u.UserId, request.UserId),
            Builders<UserDevice>.Filter.ElemMatch(u => u.Devices,
                Builders<DeviceInfoDetail>.Filter.Eq(d => d.ClientId, request.ClientId)));
            return await _userDevicesCollection.Find(filter).AnyAsync();
        }
        public async Task<ApiResult<UserViewModel>> GetById(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return new ApiErrorResult<UserViewModel>("Tài khoản không tồn tại");
            var userVm = _mapper.Map<UserViewModel>(user);
            return new ApiSuccessResult<UserViewModel>(userVm);
        }
        public async Task<ApiResult<string>> RequestUpdateMail(int userId, string newEmail)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return new ApiErrorResult<string>("Tài khoản không tồn tại");
            if (newEmail == user.Email)
                return new ApiErrorResult<string>("Email mới không thể trùng với email hiện tại");
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            await _redisService.Publish(SystemConstant.MESSAGE_UPDATE_EMAIL_EVENT, new UpdateEmailResponse
            {
                UserId = userId,
                NewEmail = newEmail,
                Token = token
            });
            return new ApiSuccessResult<string>(token);
        }
        public async Task<ApiResult<bool>> ConfirmEmailChange(UpdateEmailResponse response)
        {
            var user = await _userManager.FindByIdAsync(response.UserId.ToString());
            if (user == null) return new ApiErrorResult<bool>("Tài khoản không tồn tại");
            var decodedToken = WebUtility.UrlDecode(response.Token);
            var result = await _userManager.ChangeEmailAsync(user, response.NewEmail, decodedToken);
            if (result.Succeeded)
                return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Xác nhận thay đổi email không thành công");
        }
        public async Task<ApiResult<bool>> UpdateAvatar(int userId, IFormFile file)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return new ApiErrorResult<bool>("Tài khoản không tồn tại");
            var newAvatar = await _imageService.UploadImage(file, user.UserName!, user.UserName!);
            if (string.IsNullOrEmpty(newAvatar)) return new ApiErrorResult<bool>("Không thể lấy dữ liệu ảnh tải lên");
            user.Avatar = newAvatar;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Cập nhật ảnh đại diện không thành công");
        }
    }
}
