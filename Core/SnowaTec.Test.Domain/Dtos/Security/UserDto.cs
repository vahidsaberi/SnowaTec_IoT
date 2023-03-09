using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnowaTec.Test.Domain.DTO.Security
{
    public class UserDto
    {
        public UserDto()
        {
            Roles = new List<RoleDto>();

            PrefixSelect = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "آقای",
                    Text = "آقای"
                },
                new SelectListItem
                {
                    Value = "خانم",
                    Text = "خانم"
                },
                new SelectListItem
                {
                    Value = "شرکت",
                    Text = "شرکت"
                }
            };
        }

        public long Id { get; set; }

        [MaxLength(50)]
        [Display(Name = "پیشوند نام")]
        public string Prefix { get; set; }

        [NotMapped]
        public List<SelectListItem> PrefixSelect { get; set; }

        [MaxLength(100)]
        [Display(Name = "نام کامل")]
        [Required(ErrorMessage = "{0} را وارد کنید.")]
        public string FullName { get; set; }

        [MaxLength(50)]
        [Display(Name = "نام مستعار")]
        public string? NickName { get; set; }

        [Display(Name = "شماره تماس")]
        [Required(ErrorMessage = "{0} را وارد کنید.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "فعال")]
        public bool Active { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "{0} را وارد کنید.")]
        public string UserName { get; set; }

        [Display(Name = "گذرواژه")]
        [Required(ErrorMessage = "{0} را وارد کنید.")]
        [MinLength(6, ErrorMessage = "حداقل تعداد کاراکتر 6 است.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "تایید گذرواژه")]
        [Required(ErrorMessage = "{0} را وارد کنید.")]
        [Compare(nameof(Password), ErrorMessage = "گذرواژه و تایید آن یکسان نیست.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public List<RoleDto> Roles { get; set; }
    }
}
