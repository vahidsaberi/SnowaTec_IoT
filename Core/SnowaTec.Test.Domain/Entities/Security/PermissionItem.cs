using System.ComponentModel.DataAnnotations.Schema;

namespace SnowaTec.Test.Domain.Entities.Security
{
    [Table("PermissionItems", Schema = "security")]
    public class PermissionItem : BaseEntity
    {
        public long PermissionId { get; set; }
        public Permission Permission { get; set; }
        
        public long Key { get; set; }
        public bool Undetermined { get; set; }
    }
}
