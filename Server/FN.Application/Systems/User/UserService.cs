using AutoMapper;
using FN.Application.Helper.Images;
using FN.Application.Systems.Redis;
using FN.Application.Systems.Token;
using FN.DataAccess.Entities;
using FN.Utilities;
using FN.ViewModel.Helper.API;
using FN.ViewModel.Systems.Token;
using FN.ViewModel.Systems.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Net;

namespace FN.Application.Systems.User
{
    public class UserService : IUserService
    {
        private readonly IAuthService _authService;
        private readonly IRedisService _redisService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public UserService(IRedisService redisService,
                        IMongoDatabase database,
                        ITokenService tokenService,
                        IMapper mapper,
                        IAuthService authService,
                        IImageService imageService,
                        UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _redisService = redisService;
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _imageService = imageService;
            _authService = authService;
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
        public async Task<ApiResult<UserViewModel>> GetById(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return new ApiErrorResult<UserViewModel>("Tài khoản không tồn tại");
            var userVm = _mapper.Map<UserViewModel>(user);
            return new ApiSuccessResult<UserViewModel>(userVm);
        }
        public async Task<ApiResult<UserViewModel>> GetByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
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

        public async Task<ApiResult<string>> RequestForgotPassword(RequestForgot request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null) return new ApiErrorResult<string>("User không tồn tại");
            if(request.Email != user.Email) return new ApiErrorResult<string>("Email yêu cầu khôi phục không chính xác");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _redisService.Publish(SystemConstant.MESSAGE_FORGOT_PASSWORD_EVENT, new ForgotPasswordResponse
            {
                Username = user.UserName!,
                Email = user.Email!,
                Token = token
            });
            return new ApiSuccessResult<string>(token);
        }

        public async Task<ApiResult<bool>> ResetPassword(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null) return new ApiErrorResult<bool>("User không tồn tại");
            var decodedToken = WebUtility.UrlDecode(request.Token);
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);
            if (result.Succeeded)
            {
                await _authService.RemoveAllDevice(user.Id);
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>(result.Errors.First().Description);
        }

        public async Task<ApiResult<bool>> ChangePassword(ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null) return new ApiErrorResult<bool>("User không tồn tại");
           
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (result.Succeeded)
            {
                if (request.LogoutEverywhere)
                    await _authService.RemoveAllDevice(request.UserId);
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>(result.Errors.First().Description);
        }

        public async Task<ApiResult<bool>> ChangeName(int userId, string newName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if(user == null) return new ApiErrorResult<bool>("User không tồn tại");
            user.FullName = newName;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return new ApiSuccessResult<bool>();
            return new ApiErrorResult<bool>("Thay đổi tên không thành công");
        }

        public async Task<ApiResult<List<string>>> GetListUsername()
        {
            var users = await _userManager.Users.Select(u => u.UserName).ToListAsync();
            return new ApiSuccessResult<List<string>>(users);
        }
    }
}
