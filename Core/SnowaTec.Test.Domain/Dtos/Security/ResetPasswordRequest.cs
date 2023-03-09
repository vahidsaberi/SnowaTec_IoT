using System.ComponentModel.DataAnnotations;

namespace SnowaTec.Test.Domain.DTO.Security
{
    public class ResetPasswordRequest
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "The password must be at least 6 character long.")]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Confirm New Password")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}
