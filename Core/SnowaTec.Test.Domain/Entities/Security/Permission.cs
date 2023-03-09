using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnowaTec.Test.Domain.Entities.Security
{
    [Table("Permissions", Schema = "security")]
    public class Permission : BaseEntity
    {
        public long UserId { get; set; }

        public long RoleId { get; set; }

        public string Key { get; set; }

        public ICollection<PermissionItem> PermissionItems { get; set; }
    }
}
