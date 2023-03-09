namespace SnowaTec.Test.Domain.DTO.Security
{ 
    public class AuthenticationUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
