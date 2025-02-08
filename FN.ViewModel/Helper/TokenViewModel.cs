namespace FN.ViewModel.Helper
{
    public class TokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiry { get; set; }
    }
    public class TokenRequest
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
    }
}
