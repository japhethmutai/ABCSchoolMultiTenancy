namespace Infrastructure.Identity.Auth.Jwt
{
    public class JwtSettings
    {
        public string Key { get; set; }
        public int TokenExpiryTimeInMinutes { get; set; }
        public int RefreshTokenExpiryTimeInDays { get; set; }
    }
}
