using AutoMapper;
using SnowaTec.Test.Domain.DTO.Security;

namespace SnowaTec.Test.Web.Interfaces.Base
{
    public interface ISubPermission
    {
        Task<List<PermissionDto>> GetPermissions(ICallApiService _service, IMapper _mapper, string token, long userId, long roleId);
    }
}
