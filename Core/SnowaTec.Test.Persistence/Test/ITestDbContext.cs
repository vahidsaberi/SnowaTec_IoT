using Microsoft.EntityFrameworkCore;
using SnowaTec.Test.Domain.Entities.Test;

namespace SnowaTec.Test.Persistence.Test
{
    public interface ITestDbContext
    {
        DbSet<Section> Sections { get; set; }
        DbSet<Device> Devices { get; set; }

        Task<int> SaveChangesAsync();

        Task SetModifiedState<T>(T obj);
    }
}
