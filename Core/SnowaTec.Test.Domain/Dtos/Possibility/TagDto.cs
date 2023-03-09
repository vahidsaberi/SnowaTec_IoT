using System.ComponentModel.DataAnnotations;

namespace SnowaTec.Test.Domain.DTO.Possibility
{
    public class TagDto : BaseEntity
    {
        public long? ParentId { get; set; }

        [MaxLength(50)]
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }
    }
}
