namespace FN.ViewModel.Systems.Token
{
    public class TokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
