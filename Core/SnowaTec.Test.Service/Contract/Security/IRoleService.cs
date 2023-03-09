using System.Collections.Generic;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;

namespace SnowaTec.Test.Service.Contract.Security
{
    public interface IRoleService
    {
        Task<Response<ApplicationRole>> GetAsync(long id);
        Task<Response<List<ApplicationRole>>> GetAllAsync();
        Task<Response<List<ApplicationUser>>> GetUsersInRoleAsync(string roleName);
        Task<Response<Dictionary<string, List<ApplicationUser>>>> GetAllUsersAndRoleAsync();
        Task<Response<ApplicationRole>> CreateAsync(ApplicationRole model);
        Task<Response<ApplicationRole>> UpdateAsync(ApplicationRole model);
        Task<Response<ApplicationRole>> GetByNameAsync(string roleName);
        Task<Response<List<ApplicationRole>>> Filter(string query);
    }
}
