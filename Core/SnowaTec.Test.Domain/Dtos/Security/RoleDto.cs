using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnowaTec.Test.Domain.DTO.Security
{
    public class RoleDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }

        [Display(Name = "نقش کاربری")]
        public long RoleId { get; set; }

        public string RoleName { get; set; }

        [NotMapped]
        public int Order { get; set; }
        //public int? OrganizationChartId { get; set; }
        //public string? OrganizationChartName { get; set; }
    }
}
