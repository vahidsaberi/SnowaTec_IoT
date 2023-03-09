using Microsoft.AspNetCore.Identity;

namespace SnowaTec.Test.Domain.Entities.Security
{
    public class ApplicationRole : IdentityRole<long>
    {
        public string Key { get; set; }
    }
}
