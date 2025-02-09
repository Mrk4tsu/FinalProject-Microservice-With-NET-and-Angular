using FN.DataAccess.Entities;
using FN.ViewModel.Systems.Token;
using System.Security.Claims;

namespace FN.Application.System.Token
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(AppUser user);
        string GenerateRefreshToken();
        Task SaveRefreshToken(string refreshToken, TokenRequest request, TimeSpan expiry);
        Task RemoveRefreshToken(TokenRequest request);
        Task<string?> GetRefreshToken(TokenRequest request);
        Task<bool> IsDeviceRegistered(TokenRequest request);
        Task RegisterDevice(TokenRequest request);
        Task RemoveDevice(TokenRequest request);
    }
}
