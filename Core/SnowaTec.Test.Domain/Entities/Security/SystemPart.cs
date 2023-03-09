using SnowaTec.Test.Domain.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnowaTec.Test.Domain.Entities.Security
{
    [Table("SystemParts", Schema = "security")]
    public class SystemPart : BaseEntity
    {
        public SystemPart()
        {
            SubSystemParts = new HashSet<SystemPart>();
        }

        public SystemPart? Parent { get; set; }

        public long? ParentId { get; set; }

        [NotMapped]
        public string? ParentName { get; set; }

        [MaxLength(200)]
        public string? EnglishTitle { get; set; }

        [MaxLength(200)]
        public string PersianTitle { get; set; }

        public MenuType MenuType { get; set; }

        [MaxLength(200)]
        public string? IconName { get; set; }

        public bool AdditionalPermissions { get; set; }

        public virtual ICollection<SystemPart> SubSystemParts { get; set; }
    }
}
