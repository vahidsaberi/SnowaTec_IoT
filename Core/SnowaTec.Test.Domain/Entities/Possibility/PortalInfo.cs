using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnowaTec.Test.Domain.Entities.Possibility
{
    [Table("PortalInfo", Schema = "possibility")]
    public class PortalInfo : BaseEntity
    {
        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(150)]
        public string EnglishTitle { get; set; }

        [MaxLength(150)]
        public string SiteUrl { get; set; }

        [MaxLength(350)]
        public string Slogan { get; set; }

        [MaxLength(150)]
        public string DeveloperTitle { get; set; }

        [MaxLength(150)]
        public string DeveloperEnglishTitle { get; set; }

        [MaxLength(150)]
        public string DeveloperUrl { get; set; }

        public bool RetentionOfPreviousData { get; set; }
    }
}
