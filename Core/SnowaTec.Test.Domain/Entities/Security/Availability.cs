using System.ComponentModel.DataAnnotations.Schema;

namespace SnowaTec.Test.Domain.Entities.Security
{
    [Table("Availabilities", Schema = "security")]
    public class Availability : BaseEntity
    {
        public Availability()
        {

        }

        public long RoleId { get; set; }

        public long SystemPartId { get; set; }
        public SystemPart SystemPart { get; set; }

        public bool Undetermined { get; set; }
    }
}
