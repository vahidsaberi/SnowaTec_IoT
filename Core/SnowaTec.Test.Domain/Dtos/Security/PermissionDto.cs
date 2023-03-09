using System.Collections.Generic;

namespace SnowaTec.Test.Domain.DTO.Security
{
    public class PermissionDto : BaseEntity
    {
        public PermissionDto()
        {
            PermissionItems = new List<PermissionItemDto>();
        }

        public long UserId { get; set; }

        public long RoleId { get; set; }

        public string Key { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<PermissionItemDto> PermissionItems { get; set; }
    }
}
