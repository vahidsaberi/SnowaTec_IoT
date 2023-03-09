using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Entities.Test;

namespace SnowaTec.Test.Persistence.Test.Seeds
{
    public static class TestSeeds
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            CreateSections(modelBuilder);
        }

        private static void CreateSections(ModelBuilder modelBuilder)
        {
            List<Section> sections = DefaultSections.List();
            modelBuilder.Entity<Section>().HasData(sections);
        }
    }
}
