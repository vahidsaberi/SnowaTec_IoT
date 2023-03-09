using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Service.Features.Possibility.FieldFeatures.Commands;
using SnowaTec.Test.Service.Features.Possibility.FieldFeatures.Queries;
using System.Threading.Tasks;

namespace SnowaTec.Test.API.Controllers.Possibility
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "WebAPISpecification")]
    public class FieldController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [HttpGet(ApiRoutes.Fields.GetAll)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllFieldQuery()));
        }

        [HttpGet(ApiRoutes.Fields.Get)]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(long id)
        {
            return Ok(await Mediator.Send(new GetFieldByIdQuery { Id = id }));
        }

        [HttpPost(ApiRoutes.Fields.Create)]
        public async Task<IActionResult> Create(CreateFieldCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut(ApiRoutes.Fields.Update)]
        public async Task<IActionResult> Update(long id, UpdateFieldCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete(ApiRoutes.Fields.Delete)]
        public async Task<IActionResult> Delete(long id, int type)
        {
            return Ok(await Mediator.Send(new DeleteFieldByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.Fields.UnDelete)]
        public async Task<IActionResult> UnDelete(long id, int type)
        {
            return Ok(await Mediator.Send(new UndeleteFieldByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.Fields.Filter)]
        public async Task<IActionResult> Filter([FromBody] string query)
        {
            return Ok(await Mediator.Send(new FilterFieldQuery { Query = query }));
        }

        [HttpPost(ApiRoutes.Fields.Pagination)]
        public async Task<IActionResult> Pagination([FromBody] DTParameters parameters)
        {
            return Ok(await Mediator.Send(new DataTableFieldQuery { Parameters = parameters }));
        }
    }
}
