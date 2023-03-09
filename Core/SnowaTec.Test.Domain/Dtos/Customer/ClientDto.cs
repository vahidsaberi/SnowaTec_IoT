using System.ComponentModel.DataAnnotations;

namespace SnowaTec.Test.Domain.DTO.Customer
{
    public class ClientDto : BaseEntity
    {
        [Display(Name = "پیشوند")]
        [MaxLength(50)]
        public string Prefix { get; set; }

        [Display(Name = "نام")]
        [MaxLength(150)]
        public string Name { get; set; }

        [Display(Name = "کد ملی")]
        [MaxLength(10)]
        public string? NationalCode { get; set; }

        [Display(Name = "کد اقتصادی")]
        [MaxLength(14)]
        public string? EconomicCode { get; set; }

        [Display(Name = "شماره تماس")]
        [MaxLength(12)]
        public string? Phone { get; set; }

        [Display(Name = "آدرس")]
        public string? Address { get; set; }
    }
}
