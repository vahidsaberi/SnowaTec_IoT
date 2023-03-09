using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Service.Features.Security.AvailabilityFeatures.Commands;
using SnowaTec.Test.Service.Features.Security.AvailabilityFeatures.Queries;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Entities.Possibility;

namespace SnowaTec.Test.API.Controllers.Security
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "WebAPISpecification")]
    public class AvailabilityController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [HttpGet(ApiRoutes.Availabilities.GetAll)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllAvailabilityQuery()));
        }

        [HttpGet(ApiRoutes.Availabilities.Get)]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(long id)
        {
            return Ok(await Mediator.Send(new GetAvailabilityByIdQuery { Id = id }));
        }

        [HttpPost(ApiRoutes.Availabilities.Create)]
        public async Task<IActionResult> Create(CreateAvailabilityCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut(ApiRoutes.Availabilities.Update)]
        public async Task<IActionResult> Update(long id, UpdateAvailabilityCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete(ApiRoutes.Availabilities.Delete)]
        public async Task<IActionResult> Delete(long id, int type)
        {
            return Ok(await Mediator.Send(new DeleteAvailabilityByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.Availabilities.UnDelete)]
        public async Task<IActionResult> UnDelete(long id, int type)
        {
            return Ok(await Mediator.Send(new UndeleteAvailabilityByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.Availabilities.Filter)]
        public async Task<IActionResult> Filter([FromBody] string query)
        {
            return Ok(await Mediator.Send(new FilterAvailabilityQuery { Query = query }));
        }

        [HttpPost(ApiRoutes.Availabilities.Pagination)]
        public async Task<IActionResult> Pagination([FromBody] DTParameters parameters)
        {
            return Ok(await Mediator.Send(new DataTableAvailabilityQuery { Parameters = parameters }));
        }
    }
}
