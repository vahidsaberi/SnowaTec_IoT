using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Persistence.Identity.Seeds;

namespace SnowaTec.Test.Persistence.Identity
{
    public class IdentityContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("Identity");

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");
                entity.HasMany<ApplicationUserRole>()
                    .WithOne()
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable(name: "RefreshToken");
            });

            modelBuilder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable(name: "Role");
                entity.HasMany<ApplicationUserRole>()
                    .WithOne()
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ApplicationUserRole>(entity =>
            {
                entity.ToTable(name: "UserRoles");
            });

            modelBuilder.Entity<IdentityUserClaim<long>>(entity =>
            {
                entity.ToTable(name: "UserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<long>>(entity =>
            {
                entity.ToTable(name: "UserLogins");
            });

            modelBuilder.Entity<IdentityRoleClaim<long>>(entity =>
            {
                entity.ToTable(name: "RoleClaims");
            });

            modelBuilder.Entity<IdentityUserToken<long>>(entity =>
            {
                entity.ToTable(name: "UserTokens");
            });

            modelBuilder.Seed();
        }
    }
}
