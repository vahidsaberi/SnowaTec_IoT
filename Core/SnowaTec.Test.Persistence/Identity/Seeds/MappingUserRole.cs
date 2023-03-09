using System.Collections.Generic;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Enum;

namespace SnowaTec.Test.Persistence.Identity.Seeds
{
    public static class MappingUserRole
    {
        public static List<ApplicationUserRole> IdentityUserRoleList()
        {
            return new List<ApplicationUserRole>()
            {
                //new ApplicationUserRole
                //{
                //    RoleId = Constants.Basic,
                //    UserId = Constants.BasicUser
                //},
                new ApplicationUserRole
                {
                    RoleId = Constants.SuperAdmin,
                    UserId = Constants.SuperAdminUser
                }//,
                //new ApplicationUserRole
                //{
                //    RoleId = Constants.Admin,
                //    UserId = Constants.SuperAdminUser
                //},
                //new ApplicationUserRole
                //{
                //    RoleId = Constants.Moderator,
                //    UserId = Constants.SuperAdminUser
                //},
                //new ApplicationUserRole
                //{
                //    RoleId = Constants.Basic,
                //    UserId = Constants.SuperAdminUser
                //}
            };
        }
    }
}
