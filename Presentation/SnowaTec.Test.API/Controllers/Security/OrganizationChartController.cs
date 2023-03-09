using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Service.Features.Security.OrganizationChartFeatures.Commands;
using SnowaTec.Test.Service.Features.Security.OrganizationChartFeatures.Queries;

namespace SnowaTec.Test.API.Controllers.Security
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "WebAPISpecification")]
    public class OrganizationChartController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [HttpGet(ApiRoutes.OrganizationCharts.GetAll)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllOrganizationChartQuery()));
        }

        [HttpGet(ApiRoutes.OrganizationCharts.Get)]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(long id)
        {
            return Ok(await Mediator.Send(new GetOrganizationChartByIdQuery { Id = id }));
        }

        [HttpPost(ApiRoutes.OrganizationCharts.Create)]
        public async Task<IActionResult> Create(CreateOrganizationChartCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut(ApiRoutes.OrganizationCharts.Update)]
        public async Task<IActionResult> Update(long id, UpdateOrganizationChartCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete(ApiRoutes.OrganizationCharts.Delete)]
        public async Task<IActionResult> Delete(long id, int type)
        {
            return Ok(await Mediator.Send(new DeleteOrganizationChartByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.OrganizationCharts.UnDelete)]
        public async Task<IActionResult> UnDelete(long id, int type)
        {
            return Ok(await Mediator.Send(new UndeleteOrganizationChartByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.OrganizationCharts.Filter)]
        public async Task<IActionResult> Filter([FromBody] string query)
        {
            return Ok(await Mediator.Send(new FilterOrganizationChartQuery { Query = query }));
        }

        [HttpPost(ApiRoutes.OrganizationCharts.Pagination)]
        public async Task<IActionResult> Pagination([FromBody] DTParameters parameters)
        {
            return Ok(await Mediator.Send(new DataTableOrganizationChartQuery { Parameters = parameters }));
        }
    }
}
