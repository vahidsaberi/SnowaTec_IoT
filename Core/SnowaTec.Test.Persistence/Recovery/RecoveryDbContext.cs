using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SnowaTec.Test.Domain.Entities.Recovery;
using SnowaTec.Test.Persistence.Recovery.Seeds;
using System.Threading.Tasks;

namespace SnowaTec.Test.Persistence.Recovery
{
    public class RecoveryDbContext : DbContext, IRecoveryDbContext
    {
        // This constructor is used of runit testing
        public RecoveryDbContext()
        {

        }

        public RecoveryDbContext(DbContextOptions<RecoveryDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Backup> Backups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                .UseSqlServer("DataSource=app.db");
            }
        }

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
