using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Service.Features.Test.DeviceFeatures.Commands;
using SnowaTec.Test.Service.Features.Test.DeviceFeatures.Queries;
using SnowaTec.Test.Service.Features.Test.SectionFeatures.Queries;
using System.Threading.Tasks;

namespace SnowaTec.Test.API.Controllers.Test
{
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "WebAPISpecification")]
    public class DeviceController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [HttpGet(ApiRoutes.Devices.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllDeviceQuery()));
        }

        [HttpGet(ApiRoutes.Devices.Get)]
        public async Task<IActionResult> GetById(long id)
        {
            return Ok(await Mediator.Send(new GetDeviceByIdQuery { Id = id }));
        }

        [HttpPost(ApiRoutes.Devices.Create)]
        public async Task<IActionResult> Create(CreateDeviceCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut(ApiRoutes.Devices.Update)]
        public async Task<IActionResult> Update(long id, UpdateDeviceCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete(ApiRoutes.Devices.Delete)]
        public async Task<IActionResult> Delete(long id)
        {
            return Ok(await Mediator.Send(new DeleteDeviceByIdCommand { Id = id }));
        }

        [HttpPost(ApiRoutes.Devices.UnDelete)]
        public async Task<IActionResult> UnDelete(long id, int type)
        {
            return Ok(await Mediator.Send(new UndeleteDeviceByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.Devices.Filter)]
        public async Task<IActionResult> Filter([FromBody] string query)
        {
            return Ok(await Mediator.Send(new FilterDeviceQuery { Query = query }));
        }

        [HttpPost(ApiRoutes.Devices.Pagination)]
        public async Task<IActionResult> Pagination([FromBody] DTParameters parameters)
        {
            return Ok(await Mediator.Send(new DataTableDeviceQuery { Parameters = parameters }));
        }
    }
}
