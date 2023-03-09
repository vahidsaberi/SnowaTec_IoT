using SnowaTec.Test.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnowaTec.Test.Domain.Entities.Possibility
{
    [Table("Fields", Schema = "possibility")]
    public class Field : BaseEntity
    {
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string? Key { get; set; }

        [MaxLength(500)]
        public string? Tooltip { get; set; }

        public FieldType Type { get; set; }

        public string? DefaultValue { get; set; }

        public string Setting { get; set; }
    }
}
