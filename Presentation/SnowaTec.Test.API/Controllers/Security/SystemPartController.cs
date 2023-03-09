using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Service.Features.Security.SystemPartFeatures.Commands;
using SnowaTec.Test.Service.Features.Security.SystemPartFeatures.Queries;
using System.Threading.Tasks;

namespace SnowaTec.Test.API.Controllers.Security
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "WebAPISpecification")]
    public class SystemPartController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [HttpGet(ApiRoutes.SystemParts.GetAll)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllSystemPartQuery()));
        }

        [HttpGet(ApiRoutes.SystemParts.Get)]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(long id)
        {
            return Ok(await Mediator.Send(new GetSystemPartByIdQuery { Id = id }));
        }

        [HttpPost(ApiRoutes.SystemParts.Create)]
        public async Task<IActionResult> Create(CreateSystemPartCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut(ApiRoutes.SystemParts.Update)]
        public async Task<IActionResult> Update(long id, UpdateSystemPartCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete(ApiRoutes.SystemParts.Delete)]
        public async Task<IActionResult> Delete(long id, int type)
        {
            return Ok(await Mediator.Send(new DeleteSystemPartByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.SystemParts.UnDelete)]
        public async Task<IActionResult> UnDelete(long id, int type)
        {
            return Ok(await Mediator.Send(new UndeleteSystemPartByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.SystemParts.Filter)]
        public async Task<IActionResult> Filter([FromBody] string query)
        {
            return Ok(await Mediator.Send(new FilterSystemPartQuery { Query = query }));
        }
    }
}
