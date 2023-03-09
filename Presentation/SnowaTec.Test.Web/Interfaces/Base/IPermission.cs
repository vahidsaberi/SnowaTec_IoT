using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Enum;

namespace SnowaTec.Test.Web.Interfaces.Base
{
    public interface IPermission
    {
        Task<bool> HasAccess(string systemPart, PermissionType permission);

        Task<List<SystemPart>> GetAllAccesses();

        Task<List<long>> GetSubPermissions(string key);

        Task<bool> RetentionOfPreviousData();
    }
}
