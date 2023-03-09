using SnowaTec.Test.Domain.Enum;
using System.Threading.Tasks;

namespace SnowaTec.Test.Service.Contract.Recovery
{
    public interface IBackupService
    {
        Task Save(string schema, string tableName, long key, dynamic data, ActionType actionType);
    }
}
