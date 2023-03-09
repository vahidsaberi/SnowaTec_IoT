using System.ComponentModel.DataAnnotations.Schema;

namespace SnowaTec.Test.Domain.Entities.Test
{
    [Table("Devices", Schema = "tst")]
    public class Device : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ClientId { get; set; }
        public string Topic { get; set; }
    }
}
