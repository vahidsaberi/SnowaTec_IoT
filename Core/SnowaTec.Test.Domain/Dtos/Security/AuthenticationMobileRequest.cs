namespace SnowaTec.Test.Domain.DTO.Security
{
    public class AuthenticationMobileRequest
    {
        public string PhoneNumber { get; set; }
        public string VerifyCode { get; set; }

        public bool RememberMe { get; set; }
    }
}
