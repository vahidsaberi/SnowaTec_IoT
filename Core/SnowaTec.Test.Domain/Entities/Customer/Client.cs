using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnowaTec.Test.Domain.Entities.Customer
{
    [Table("Clients", Schema = "customer")]
    public class Client : BaseEntity
    {
        [MaxLength(50)]
        public string Prefix { get; set; }

        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(10)]
        public string? NationalCode { get; set; }

        [MaxLength(14)]
        public string? EconomicCode { get; set; }

        [MaxLength(12)]
        public string? Phone { get; set; }

        public string? Address { get; set; }
    }
}
