using System.Collections.Generic;

namespace SnowaTec.Test.Domain.DTO.Security
{
    public class PermissionItemDto : BaseEntity
    {
        public long PermissionId { get; set; }
        public long Key { get; set; }
        public long? ParentKey { get; set; }
        public string Title { get; set; }
        public bool Selected { get; set; }
        public bool Undetermined { get; set; }

        public List<PermissionItemDto> SubPermissionItems { get; set; }
    }
}
