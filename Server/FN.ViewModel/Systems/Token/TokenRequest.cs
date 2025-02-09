namespace FN.ViewModel.Systems.Token
{
    public class TokenRequest
    {
        public int UserId { get; set; }
        public string ClientId { get; set; } = string.Empty;
    }
    public class RefreshTokenRequest : TokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
