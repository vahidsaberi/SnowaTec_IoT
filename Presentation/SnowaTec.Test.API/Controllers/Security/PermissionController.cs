using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Service.Features.Security.PermissionFeatures.Commands;
using SnowaTec.Test.Service.Features.Security.PermissionFeatures.Queries;
using System.Threading.Tasks;

namespace SnowaTec.Test.API.Controllers.Security
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "WebAPISpecification")]
    public class PermissionController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [HttpGet(ApiRoutes.Permissions.GetAll)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllPermissionQuery()));
        }

        [HttpGet(ApiRoutes.Permissions.Get)]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(long id)
        {
            return Ok(await Mediator.Send(new GetPermissionByIdQuery { Id = id }));
        }

        [HttpPost(ApiRoutes.Permissions.Create)]
        public async Task<IActionResult> Create(CreatePermissionCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut(ApiRoutes.Permissions.Update)]
        public async Task<IActionResult> Update(long id, UpdatePermissionCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete(ApiRoutes.Permissions.Delete)]
        public async Task<IActionResult> Delete(long id, int type)
        {
            return Ok(await Mediator.Send(new DeletePermissionByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.Permissions.UnDelete)]
        public async Task<IActionResult> UnDelete(long id, int type)
        {
            return Ok(await Mediator.Send(new UndeletePermissionByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.Permissions.Filter)]
        public async Task<IActionResult> Filter([FromBody] string query)
        {
            return Ok(await Mediator.Send(new FilterPermissionQuery { Query = query }));
        }
    }
}
