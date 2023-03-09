using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnowaTec.Test.Domain.Entities.Test
{
    [Table("Sections", Schema = "tst")]
    public class Section : BaseEntity
    {
        public Section? Parent { get; set; }
        public long? ParentId { get; set; }

        [MaxLength(10)]
        public string Code { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        public string? Description { get; set; }

        public ICollection<Section> SubSections { get; set; }
    }
}
