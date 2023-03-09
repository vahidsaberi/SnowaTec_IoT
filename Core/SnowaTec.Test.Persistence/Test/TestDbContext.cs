using SnowaTec.Test.Persistence.Test.Seeds;
using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Entities.Test;

namespace SnowaTec.Test.Persistence.Test
{

    public class TestDbContext : DbContext, ITestDbContext
    {
        // This constructor is used of runit testing
        public TestDbContext()
        {

        }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Section>().Property(f => f.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Device>().Property(f => f.Id).ValueGeneratedOnAdd();
            
            modelBuilder.Seed();
        }

        public DbSet<Section> Sections { get; set; }
        public DbSet<Device> Devices { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public async Task SetModifiedState<T>(T obj)
        {
            this.Entry(obj).State = EntityState.Modified;
        }
    }
}
