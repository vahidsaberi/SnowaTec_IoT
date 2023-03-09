using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Entities.Customer;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Portal.Seeds;

namespace SnowaTec.Test.Persistence.Portal
{
    public class PortalDbContext : DbContext, IPortalDbContext
    {
        // This constructor is used of runit testing
        public PortalDbContext()
        {

        }

        public PortalDbContext(DbContextOptions<PortalDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        #region Possibility
        public DbSet<PortalInfo> PortalInfos { get; set; }
        public DbSet<Document> Documents { get; set; }
        #endregion

        #region Security
        public DbSet<SystemPart> SystemParts { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<OrganizationChart> OrganizationCharts { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionItem> PermissionItems { get; set; }
        #endregion

        #region Portal

        #endregion

        #region Customer
        public DbSet<Client> Clients { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.Restrict;
            //}

            #region Security
            modelBuilder.Entity<SystemPart>()
                   .HasMany(j => j.SubSystemParts)
                   .WithOne(j => j.Parent)
                   .HasForeignKey(j => j.ParentId);
            #endregion

            #region Portal

            #endregion

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
