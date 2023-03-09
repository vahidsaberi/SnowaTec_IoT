using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Entities.Recovery;
using System.Threading.Tasks;

namespace SnowaTec.Test.Persistence.Recovery
{
    public interface IRecoveryDbContext
    {
        DbSet<Backup> Backups { get; set; }

        Task<int> SaveChangesAsync();

        Task SetModifiedState<T>(T obj);
    }
}
