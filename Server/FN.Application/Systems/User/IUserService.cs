using FN.ViewModel.Helper.API;
using FN.ViewModel.Helper.Device;
using FN.ViewModel.Systems.Token;
using FN.ViewModel.Systems.User;
using Microsoft.AspNetCore.Http;

namespace FN.Application.Systems.User
{
    public interface IUserService
    {
        Task<ApiResult<TokenResponse>> RefreshToken(RefreshTokenRequest request);
        Task<ApiResult<bool>> Register(RegisterDTO request);
        Task<ApiResult<TokenResponse>> Authenticate(LoginDTO request, HttpContext context);
        Task<ApiResult<bool>> RevokeDevice(TokenRequest request);
        Task SaveDeviceInfo(TokenRequest request, string userAgent, string ipAddress);
        Task<ApiResult<List<DeviceInfoDetail>>> GetRegisteredDevices(int userId);
        Task<bool> IsDeviceRegistered(TokenRequest request);

        Task<ApiResult<UserViewModel>> GetById(int id);
        Task<ApiResult<bool>> UpdateAvatar(int userId, IFormFile file);
        Task<ApiResult<bool>> ConfirmEmailChange(UpdateEmailResponse response);
        Task<ApiResult<string>> RequestUpdateMail(int userId, string newEmail);
        Task<ApiResult<string>> RequestForgotPassword(string email);
    }
}
