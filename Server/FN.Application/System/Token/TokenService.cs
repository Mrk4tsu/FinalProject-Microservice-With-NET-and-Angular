using FN.Application.System.Redis;
using FN.DataAccess.Entities;
using FN.ViewModel.Systems.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FN.Application.System.Token
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRedisService _redisService;
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration,
            UserManager<AppUser> userManager,
            IRedisService redisService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _redisService = redisService;
        }
        public async Task<string> GenerateAccessToken(AppUser user)
        {
            var keyString = _configuration["Tokens:Key"] ?? throw new ArgumentNullException("Tokens:Key", "Token key must be configured.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var role = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("FullName", user.FullName),
                new Claim("Avatar", user.Avatar),
                new Claim("Role", string.Join(';', role))
            };

            var expiryMinutesString = _configuration["Tokens:AccessTokenExpiryMinutes"];
            if (!double.TryParse(expiryMinutesString, out double expiryMinutes)) expiryMinutes = 30;

            var token = new JwtSecurityToken(
                        issuer: _configuration["Tokens:Issuer"],
                        audience: _configuration["Tokens:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(expiryMinutes),
                        signingCredentials: creds
                    );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerateRefreshToken() => Guid.NewGuid().ToString();
        public async Task<string?> GetRefreshToken(TokenRequest request)
        {
            var key = $"auth:{request.UserId}:refresh_token:{request.ClientId}";
            return await _redisService.GetValue<string>(key);
        }
        public async Task RemoveRefreshToken(TokenRequest request)
        {
            var key = $"auth:{request.UserId}:refresh_token:{request.ClientId}";
            if (await _redisService.KeyExist(key))
                await _redisService.RemoveValue(key);
        }
        public async Task SaveRefreshToken(string refreshToken, TokenRequest request, TimeSpan expiry)
        {
            var key = $"auth:{request.UserId}:refresh_token:{request.ClientId}";
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry
            };
            await _redisService.SetValue(key, refreshToken, expiry);
        }     
    }
}
