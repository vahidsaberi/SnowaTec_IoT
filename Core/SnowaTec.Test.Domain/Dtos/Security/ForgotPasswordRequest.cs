using System.ComponentModel.DataAnnotations;

namespace SnowaTec.Test.Domain.DTO.Security
{
    public class ForgotPasswordRequest
    {
        //[Required]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required]
        public string Username { get; set; }
    }
}
