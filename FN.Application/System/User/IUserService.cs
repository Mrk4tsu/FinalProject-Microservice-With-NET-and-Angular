using FN.ViewModel.Helper;
using FN.ViewModel.Systems.Token;
using FN.ViewModel.Systems.User;

namespace FN.Application.System.User
{
    public interface IUserService
    {
        Task<ApiResult<TokenResponse>> RefreshToken(RefreshTokenRequest request);
        Task<ApiResult<bool>> Register(RegisterDTO request);
        Task<ApiResult<TokenResponse>> Authenticate(LoginDTO request);
    }
}
