using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Service.Features.Portal.ClientFeatures.Commands;
using SnowaTec.Test.Service.Features.Portal.ClientFeatures.Queries;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Entities.Possibility;

namespace SnowaTec.Test.API.Controllers.Portal
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "WebAPISpecification")]
    public class ClientController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [HttpGet(ApiRoutes.Clients.GetAll)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllClientQuery()));
        }

        [HttpGet(ApiRoutes.Clients.Get)]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(long id)
        {
            return Ok(await Mediator.Send(new GetClientByIdQuery { Id = id }));
        }

        [HttpPost(ApiRoutes.Clients.Create)]
        public async Task<IActionResult> Create(CreateClientCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut(ApiRoutes.Clients.Update)]
        public async Task<IActionResult> Update(long id, UpdateClientCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete(ApiRoutes.Clients.Delete)]
        public async Task<IActionResult> Delete(long id, int type)
        {
            return Ok(await Mediator.Send(new DeleteClientByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.Clients.UnDelete)]
        public async Task<IActionResult> UnDelete(long id, int type)
        {
            return Ok(await Mediator.Send(new UndeleteClientByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.Clients.Filter)]
        public async Task<IActionResult> Filter([FromBody] string query)
        {
            return Ok(await Mediator.Send(new FilterClientQuery { Query = query }));
        }

        [HttpPost(ApiRoutes.Clients.Pagination)]
        public async Task<IActionResult> Pagination([FromBody] DTParameters parameters)
        {
            return Ok(await Mediator.Send(new DataTableClientQuery { Parameters = parameters }));
        }
    }
}
