using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Entities.Customer;

namespace SnowaTec.Test.Persistence.Portal
{
    public interface IPortalDbContext
    {
        #region Possibility
        DbSet<PortalInfo> PortalInfos { get; set; }
        DbSet<Document> Documents { get; set; }
        #endregion

        #region Security
        DbSet<SystemPart> SystemParts { get; set; }
        DbSet<Availability> Availabilities { get; set; }
        DbSet<OrganizationChart> OrganizationCharts { get; set; }
        DbSet<Field> Fields { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<Permission> Permissions { get; set; }
        DbSet<PermissionItem> PermissionItems { get; set; }
        #endregion

        #region Portal

        #endregion

        #region Customer
        DbSet<Client> Clients { get; set; }
        #endregion

        Task<int> SaveChangesAsync();

        Task SetModifiedState<T>(T obj);
    }
}
