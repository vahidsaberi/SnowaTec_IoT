using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Domain.Entities.Security;

namespace SnowaTec.Test.Persistence.Portal.Seeds
{
    public static class PortalSeed
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            CreatePortalInfos(modelBuilder);
            CreateSystemParts(modelBuilder);
            CreateTags(modelBuilder);
            CreateFields(modelBuilder);
        }

        private static void CreatePortalInfos(ModelBuilder modelBuilder)
        {
            List<PortalInfo> portalInfos = DefaultPortalInfos.PortalInfoList();
            modelBuilder.Entity<PortalInfo>().HasData(portalInfos);
        }

        private static void CreateSystemParts(ModelBuilder modelBuilder)
        {
            List<SystemPart> systemParts = DefaultSystemParts.SystemPartList();
            modelBuilder.Entity<SystemPart>().HasData(systemParts);
        }

        private static void CreateTags(ModelBuilder modelBuilder)
        {
            List<Tag> tags = DefaultTags.List();
            modelBuilder.Entity<Tag>().HasData(tags);
        }

        private static void CreateFields(ModelBuilder modelBuilder)
        {
            List<Field> fields = DefaultFields.List();
            modelBuilder.Entity<Field>().HasData(fields);
        }
    }
}
