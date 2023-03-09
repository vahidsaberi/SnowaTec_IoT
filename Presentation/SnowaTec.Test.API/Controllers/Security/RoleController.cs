using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Service.Contract.Security;
using System.Threading.Tasks;

namespace SnowaTec.Test.API.Controllers.Security
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "WebAPISpecification")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet(ApiRoutes.Roles.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _roleService.GetAllAsync());
        }

        [HttpGet(ApiRoutes.Roles.Get)]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(long id)
        {
            return Ok(await _roleService.GetAsync(id));
        }

        [HttpGet(ApiRoutes.Roles.GetUsersInRole)]
        public async Task<IActionResult> GetUsersInRole(string roleName)
        {
            return Ok(await _roleService.GetUsersInRoleAsync(roleName));
        }

        [HttpGet(ApiRoutes.Roles.GetAllUsersAndRole)]
        public async Task<IActionResult> GetAllUsersAndRole()
        {
            return Ok(await _roleService.GetAllUsersAndRoleAsync());
        }

        [HttpPost(ApiRoutes.Roles.Create)]
        public async Task<IActionResult> Create(ApplicationRole command)
        {
            return Ok(await _roleService.CreateAsync(command));
        }

        [HttpPut(ApiRoutes.Roles.Update)]
        public async Task<IActionResult> Update(string id, ApplicationRole command)
        {
            return Ok(await _roleService.UpdateAsync(command));
        }

        [HttpGet(ApiRoutes.Roles.GetByName)]
        public async Task<IActionResult> UpGetByNamedate(string name)
        {
            return Ok(await _roleService.GetByNameAsync(name));
        }

        [HttpPost(ApiRoutes.Roles.Filter)]
        public async Task<IActionResult> Filter([FromBody] string query)
        {
            return Ok(await _roleService.Filter(query));
        }
    }
}
