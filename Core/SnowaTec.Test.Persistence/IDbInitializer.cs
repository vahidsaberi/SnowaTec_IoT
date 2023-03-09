using Microsoft.EntityFrameworkCore;

namespace SnowaTec.Test.Persistence
{
    public interface IDbInitializer<T> where T : DbContext
    {
        void Initialize();
    }
}
