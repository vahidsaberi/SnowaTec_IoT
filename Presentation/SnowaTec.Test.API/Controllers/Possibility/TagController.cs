using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Service.Features.Possibility.TagFeatures.Commands;
using SnowaTec.Test.Service.Features.Possibility.TagFeatures.Queries;
using System.Threading.Tasks;

namespace SnowaTec.Test.API.Controllers.Possibility
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "WebAPISpecification")]
    public class TagController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [HttpGet(ApiRoutes.Tags.GetAll)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllTagQuery()));
        }

        [HttpGet(ApiRoutes.Tags.Get)]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(long id)
        {
            return Ok(await Mediator.Send(new GetTagByIdQuery { Id = id }));
        }

        [HttpPost(ApiRoutes.Tags.Create)]
        public async Task<IActionResult> Create(CreateTagCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut(ApiRoutes.Tags.Update)]
        public async Task<IActionResult> Update(long id, UpdateTagCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete(ApiRoutes.Tags.Delete)]
        public async Task<IActionResult> Delete(long id, int type)
        {
            return Ok(await Mediator.Send(new DeleteTagByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.Tags.UnDelete)]
        public async Task<IActionResult> UnDelete(long id, int type)
        {
            return Ok(await Mediator.Send(new UndeleteTagByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.Tags.Filter)]
        public async Task<IActionResult> Filter([FromBody] string query)
        {
            return Ok(await Mediator.Send(new FilterTagQuery { Query = query }));
        }

        [HttpPost(ApiRoutes.Tags.Pagination)]
        public async Task<IActionResult> Pagination([FromBody] DTParameters parameters)
        {
            return Ok(await Mediator.Send(new DataTableTagQuery { Parameters = parameters }));
        }
    }
}
