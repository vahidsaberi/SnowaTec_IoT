using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Service.Features.PortalInfoFeatures.Commands;
using SnowaTec.Test.Service.Features.PortalInfoFeatures.Queries;
using System.Threading.Tasks;

namespace SnowaTec.Test.API.Controllers.Possibility
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "WebAPISpecification")]
    public class PortalInfoController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [HttpGet(ApiRoutes.PortalInfos.GetAll)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllPortalInfoQuery()));
        }

        [HttpGet(ApiRoutes.PortalInfos.Get)]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(long id)
        {
            return Ok(await Mediator.Send(new GetPortalInfoByIdQuery { Id = id }));
        }

        [HttpPost(ApiRoutes.PortalInfos.Create)]
        public async Task<IActionResult> Create(CreatePortalInfoCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut(ApiRoutes.PortalInfos.Update)]
        public async Task<IActionResult> Update(long id, UpdatePortalInfoCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete(ApiRoutes.PortalInfos.Delete)]
        public async Task<IActionResult> Delete(long id, int type)
        {
            return Ok(await Mediator.Send(new DeletePortalInfoByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.PortalInfos.UnDelete)]
        public async Task<IActionResult> UnDelete(long id, int type)
        {
            return Ok(await Mediator.Send(new UndeletePortalInfoByIdCommand { Id = id, Type = type }));
        }
    }
}
