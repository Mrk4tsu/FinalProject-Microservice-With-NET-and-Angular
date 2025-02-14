using FN.ViewModel.Helper.API;
using FN.ViewModel.Helper.Device;
using FN.ViewModel.Systems.Token;
using FN.ViewModel.Systems.User;

namespace FN.Application.System.User
{
    public interface IUserService
    {
        Task<ApiResult<TokenResponse>> RefreshToken(RefreshTokenRequest request);
        Task<ApiResult<bool>> Register(RegisterDTO request);
        Task<ApiResult<TokenResponse>> Authenticate(LoginDTO request);
        Task<ApiResult<bool>> RevokeDevice(TokenRequest request);

        Task SaveDeviceInfo(TokenRequest request, string userAgent, string ipAddress);
        Task<ApiResult<List<DeviceInfoDetail>>> GetRegisteredDevices(int userId);
        Task<bool> IsDeviceRegistered(TokenRequest request);
    }
}
