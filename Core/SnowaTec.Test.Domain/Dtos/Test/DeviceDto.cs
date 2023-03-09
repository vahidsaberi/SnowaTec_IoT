using System.ComponentModel.DataAnnotations;

namespace SnowaTec.Test.Domain.Dtos.Test
{
    public class DeviceDto : BaseEntity
    {
        [Display(Name = "نام دستگاه")]
        public string Name { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "شناسه")]
        public string ClientId { get; set; }

        [Display(Name = "نام کانال ارتباطی")]
        public string Topic { get; set; }
    }
}
