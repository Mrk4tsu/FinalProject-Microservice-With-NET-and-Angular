using FN.DataAccess.Entities;
using FN.ViewModel.Systems.Token;

namespace FN.Application.Systems.Token
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(AppUser user);
        string GenerateRefreshToken();
        Task SaveRefreshToken(string refreshToken, TokenRequest request, TimeSpan expiry);
        Task RemoveRefreshToken(TokenRequest request);
        Task RemoveAllTokensForUser(int userId);
        Task<string?> GetRefreshToken(TokenRequest request);
    }
}
