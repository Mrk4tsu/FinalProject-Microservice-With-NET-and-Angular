using FN.Utilities.Device;
using FN.ViewModel.Helper.API;
using FN.ViewModel.Systems.Token;
using FN.ViewModel.Systems.User;
using Microsoft.AspNetCore.Http;

namespace FN.Application.Systems.User
{
    public interface IAuthService
    {
        Task<ApiResult<TokenResponse>> RefreshToken(RefreshTokenRequest request);
        Task<ApiResult<bool>> Register(RegisterDTO request);
        Task<ApiResult<TokenResponse>> Authenticate(LoginDTO request, HttpContext context);
        Task<ApiResult<bool>> RemoveAllDevice(int userId);
        Task<ApiResult<bool>> RevokeDevice(TokenRequest request);
        Task SaveDeviceInfo(TokenRequest request, string userAgent, string ipAddress);
        Task<ApiResult<List<DeviceInfoDetail>>> GetRegisteredDevices(int userId);
        Task<bool> IsDeviceRegistered(TokenRequest request);
    }
}
