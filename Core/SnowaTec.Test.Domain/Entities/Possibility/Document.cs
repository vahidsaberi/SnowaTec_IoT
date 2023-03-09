using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SnowaTec.Test.Domain.Enum;

namespace SnowaTec.Test.Domain.Entities.Possibility
{
    [Table("Documents", Schema = "possibility")]
    public class Document : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string ContentType { get; set; }
        [MaxLength(5)]
        public string Extension { get; set; }

        public long Size { get; set; }

        [MaxLength]
        public byte[] Content { get; set; }

        public string? Key { get; set; }

        public ModuleType? ModuleId { get; set; }
        public long? ModuleRecordId { get; set; }
        public long? ModuleSubRecordId { get; set; }
        public long UserId { get; set; }
        public int Order { get; set; }
        public string? Tags { get; set; }
    }
}
