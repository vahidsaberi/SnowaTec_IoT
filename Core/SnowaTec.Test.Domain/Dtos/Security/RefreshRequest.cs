namespace SnowaTec.Test.Domain.DTO.Security
{
    public class RefreshRequest
    {
        public RefreshRequest(string jwtoken, string refreshToken)
        {
            JWToken = jwtoken;
            RefreshToken = refreshToken;
        }

        public string JWToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
