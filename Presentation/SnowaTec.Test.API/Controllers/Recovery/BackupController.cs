using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Service.Features.Recovery.BackupFeatures.Commands;
using SnowaTec.Test.Service.Features.Recovery.BackupFeatures.Queries;
using System.Threading.Tasks;

namespace SnowaTec.Test.API.Controllers.Recovery
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "WebAPISpecification")]
    public class BackupController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [HttpGet(ApiRoutes.Backups.GetAll)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllBackupQuery()));
        }

        [HttpGet(ApiRoutes.Backups.Get)]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(long id)
        {
            return Ok(await Mediator.Send(new GetBackupByIdQuery { Id = id }));
        }

        [HttpPost(ApiRoutes.Backups.Create)]
        public async Task<IActionResult> Create(CreateBackupCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut(ApiRoutes.Backups.Update)]
        public async Task<IActionResult> Update(long id, UpdateBackupCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete(ApiRoutes.Backups.Delete)]
        public async Task<IActionResult> Delete(long id, int type)
        {
            return Ok(await Mediator.Send(new DeleteBackupByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.Backups.UnDelete)]
        public async Task<IActionResult> UnDelete(long id, int type)
        {
            return Ok(await Mediator.Send(new UndeleteBackupByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.Backups.Filter)]
        public async Task<IActionResult> Filter([FromBody] string query)
        {
            return Ok(await Mediator.Send(new FilterBackupQuery { Query = query }));
        }

        [HttpPost(ApiRoutes.Backups.Pagination)]
        public async Task<IActionResult> Pagination([FromBody] DTParameters parameters)
        {
            return Ok(await Mediator.Send(new DataTableBackupQuery { Parameters = parameters }));
        }
    }
}
