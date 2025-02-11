using FN.DataAccess.Entities;
using FN.ViewModel.Helper.API;
using FN.ViewModel.Helper.Device;
using FN.ViewModel.Systems.Token;
using System.Security.Claims;

namespace FN.Application.Systems.Token
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(AppUser user);
        string GenerateRefreshToken();
        Task SaveRefreshToken(string refreshToken, TokenRequest request, TimeSpan expiry);
        Task RemoveRefreshToken(TokenRequest request);
        Task<string?> GetRefreshToken(TokenRequest request);
    }
}
