namespace Dynamic_RBAMS.DTOs.AuthDtos
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiry { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
        public string Message { get; set; }
    }
}
