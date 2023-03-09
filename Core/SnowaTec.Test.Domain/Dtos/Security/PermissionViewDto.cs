using System.Collections.Generic;

namespace SnowaTec.Test.Domain.DTO.Security
{
    public class PermissionViewDto
    {
        public string Key { get; set; }
        public List<string> UndeterminedLicenses { get; set; }
        public List<string> SelectedLicenses { get; set; }
    }
}
