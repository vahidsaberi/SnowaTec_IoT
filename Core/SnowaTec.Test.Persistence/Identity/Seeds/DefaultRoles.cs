using System.Collections.Generic;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Enum;

namespace SnowaTec.Test.Persistence.Identity.Seeds
{

    public static class DefaultRoles
    {
        public static List<ApplicationRole> ApplicationRoleList()
        {
            return new List<ApplicationRole>()
            {
                new ApplicationRole
                {
                    Id = Constants.SuperAdmin,
                    Name = Roles.SuperAdmin.ToString(),
                    NormalizedName = Roles.SuperAdmin.ToString()
                }//,
                //new ApplicationRole
                //{
                //    Id = Constants.Admin,
                //    Name = Roles.Admin.ToString(),
                //    NormalizedName = Roles.Admin.ToString()
                //},
                //new ApplicationRole
                //{
                //    Id = Constants.Moderator,
                //    Name = Roles.Moderator.ToString(),
                //    NormalizedName = Roles.Moderator.ToString()
                //},
                //new ApplicationRole
                //{
                //    Id = Constants.Basic,
                //    Name = Roles.Basic.ToString(),
                //    NormalizedName = Roles.Basic.ToString()
                //}
            };
        }
    }
}
