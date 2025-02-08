using FN.DataAccess.Entities;
using System.Security.Claims;

namespace FN.Application.System.Token
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(AppUser user);
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        Task SaveRefreshToken(string refreshToken, int userId, TimeSpan expiry);
        Task RemoveRefreshToken(int userId);
        Task<string?> GetRefreshToken(int userId);
    }
}
