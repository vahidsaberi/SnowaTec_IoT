using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Service.Features.Test.SectionFeatures.Commands;
using SnowaTec.Test.Service.Features.Test.SectionFeatures.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Entities.Possibility;

namespace SnowaTec.Test.API.Controllers.Test
{
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "WebAPISpecification")]
    public class SectionController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [HttpGet(ApiRoutes.Sections.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllSectionQuery()));
        }

        [HttpGet(ApiRoutes.Sections.Get)]
        public async Task<IActionResult> GetById(long id)
        {
            return Ok(await Mediator.Send(new GetSectionByIdQuery { Id = id }));
        }

        [HttpPost(ApiRoutes.Sections.Create)]
        public async Task<IActionResult> Create(CreateSectionCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut(ApiRoutes.Sections.Update)]
        public async Task<IActionResult> Update(long id, UpdateSectionCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete(ApiRoutes.Sections.Delete)]
        public async Task<IActionResult> Delete(long id)
        {
            return Ok(await Mediator.Send(new DeleteSectionByIdCommand { Id = id }));
        }

        [HttpPost(ApiRoutes.Sections.UnDelete)]
        public async Task<IActionResult> UnDelete(long id, int type)
        {
            return Ok(await Mediator.Send(new UndeleteSectionByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.Sections.Filter)]
        public async Task<IActionResult> Filter([FromBody] string query)
        {
            return Ok(await Mediator.Send(new FilterSectionQuery { Query = query }));
        }

        [HttpPost(ApiRoutes.Sections.Pagination)]
        public async Task<IActionResult> Pagination([FromBody] DTParameters parameters)
        {
            return Ok(await Mediator.Send(new DataTableSectionQuery { Parameters = parameters }));
        }
    }
}
