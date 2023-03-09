using EFCore.AutomaticMigrations;
using Microsoft.EntityFrameworkCore;

namespace SnowaTec.Test.Persistence
{
    public class DbInitializer<T> : IDbInitializer<T> where T : DbContext
    {
        private readonly T _context;

        public DbInitializer(T context)
        {
            _context = context;
        }

        public async void Initialize()
        {
            try
            {
                await (_context as DbContext).MigrateToLatestVersionAsync(new DbMigrationsOptions { ResetDatabaseSchema = false });
            }
            catch (Exception ex)
            {

            }
        }
    }
}
