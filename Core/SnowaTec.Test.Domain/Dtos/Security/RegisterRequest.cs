using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SnowaTec.Test.Domain.Enum;

namespace SnowaTec.Test.Domain.DTO.Security
{
    public class RegisterRequest
    {
        [Required]
        [MinLength(6)]
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public List<string> Roles { get; set; }

        [Required]
        [MaxLength(50)]
        public string Prefix { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(50)]
        public string NickName { get; set; }

        [DefaultValue(false)]
        public bool LockoutEnabled { get; set; }

        public EducationType EducationType { get; set; }
        public string EmergencyPhone { get; set; }
        public string Address { get; set; }
    }
}
