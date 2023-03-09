using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Service.Features.Security.PermissionItemFeatures.Commands;
using SnowaTec.Test.Service.Features.Security.PermissionItemFeatures.Queries;
using System.Threading.Tasks;

namespace SnowaTec.Test.API.Controllers.Security
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "WebAPISpecification")]
    public class PermissionItemController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [HttpGet(ApiRoutes.PermissionItems.GetAll)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllPermissionItemQuery()));
        }

        [HttpGet(ApiRoutes.PermissionItems.Get)]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(long id)
        {
            return Ok(await Mediator.Send(new GetPermissionItemByIdQuery { Id = id }));
        }

        [HttpPost(ApiRoutes.PermissionItems.Create)]
        public async Task<IActionResult> Create(CreatePermissionItemCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut(ApiRoutes.PermissionItems.Update)]
        public async Task<IActionResult> Update(long id, UpdatePermissionItemCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete(ApiRoutes.PermissionItems.Delete)]
        public async Task<IActionResult> Delete(long id, int type)
        {
            return Ok(await Mediator.Send(new DeletePermissionItemByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.PermissionItems.UnDelete)]
        public async Task<IActionResult> UnDelete(long id, int type)
        {
            return Ok(await Mediator.Send(new UndeletePermissionItemByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.PermissionItems.Filter)]
        public async Task<IActionResult> Filter([FromBody] string query)
        {
            return Ok(await Mediator.Send(new FilterPermissionItemQuery { Query = query }));
        }
    }
}
