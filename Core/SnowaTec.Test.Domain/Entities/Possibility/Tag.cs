using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnowaTec.Test.Domain.Entities.Possibility
{
    [Table("Tags", Schema = "possibility")]
    public class Tag : BaseEntity
    {
        public Tag? Parent { get; set; }

        public long? ParentId { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        public string? Description { get; set; }

        public virtual ICollection<Tag> SubTags { get; set; }
    }
}
