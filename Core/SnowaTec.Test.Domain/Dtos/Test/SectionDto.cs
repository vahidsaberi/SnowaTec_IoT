using System.ComponentModel.DataAnnotations;

namespace SnowaTec.Test.Domain.Dtos.Test
{
    public class SectionDto : BaseEntity
    {
        public long? ParentId { get; set; }

        [Display(Name = "کد")]
        [MaxLength(10)]
        public string Code { get; set; }

        [Display(Name = "نام")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }
    }
}
