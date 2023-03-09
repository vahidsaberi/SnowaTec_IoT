using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Entities.Security;

namespace SnowaTec.Test.Persistence.Identity.Seeds
{
    public static class IdentitySeed
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            CreateRoles(modelBuilder);

            CreateBasicUsers(modelBuilder);

            MapUserRole(modelBuilder);
        }

        private static void CreateRoles(ModelBuilder modelBuilder)
        {
            List<ApplicationRole> roles = DefaultRoles.ApplicationRoleList();
            modelBuilder.Entity<ApplicationRole>().HasData(roles);
        }

        private static void CreateBasicUsers(ModelBuilder modelBuilder)
        {
            List<ApplicationUser> users = DefaultUser.IdentityBasicUserList();
            modelBuilder.Entity<ApplicationUser>().HasData(users);
        }

        private static void MapUserRole(ModelBuilder modelBuilder)
        {
            var identityUserRoles = MappingUserRole.IdentityUserRoleList();
            modelBuilder.Entity<ApplicationUserRole>().HasData(identityUserRoles);
        }


    }
}
