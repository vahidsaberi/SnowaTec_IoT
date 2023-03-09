using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnowaTec.Test.Domain.Entities.Security
{
    [Table("OrganizationCharts", Schema = "portal")]
    public class OrganizationChart : BaseEntity
    {
        public OrganizationChart Parent { get; set; }

        public long? ParentId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<OrganizationChart> SubOrganizationCharts { get; set; }
    }
}
