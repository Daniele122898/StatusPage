namespace StatusPageAPI.Models.Configurations
{
    public class AuthenticationConfig
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string TokenIssuer { get; set; }
        public int DaysUntilTokenExpiration { get; set; }
        public int EntityCooldownSeconds { get; set; }
    }
}