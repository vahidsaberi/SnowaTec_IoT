using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Possibility;
using SnowaTec.Test.Service.Features.Security.UserFeatures.Commands;
using SnowaTec.Test.Service.Features.Security.UserFeatures.Queries;

namespace SnowaTec.Test.API.Controllers.Security
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "WebAPISpecification")]
    public class UserController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [HttpGet(ApiRoutes.Users.GetAll)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllUserQuery()));
        }

        [HttpGet(ApiRoutes.Users.Get)]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(long id)
        {
            return Ok(await Mediator.Send(new GetUserByIdQuery { Id = id }));
        }

        [HttpPost(ApiRoutes.Users.Create)]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut(ApiRoutes.Users.Update)]
        public async Task<IActionResult> Update(long id, UpdateUserCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete(ApiRoutes.Users.Delete)]
        public async Task<IActionResult> Delete(long id, int type)
        {
            return Ok(await Mediator.Send(new DeleteUserByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.Users.UnDelete)]
        public async Task<IActionResult> UnDelete(long id, int type)
        {
            return Ok(await Mediator.Send(new UndeleteUserByIdCommand { Id = id, Type = type }));
        }

        [HttpPost(ApiRoutes.Users.Filter)]
        public async Task<IActionResult> Filter([FromBody] string query)
        {
            return Ok(await Mediator.Send(new FilterUserQuery { Query = query }));
        }

        [HttpPost(ApiRoutes.Users.Pagination)]
        public async Task<IActionResult> Pagination([FromBody] DTParameters parameters)
        {
            return Ok(await Mediator.Send(new DataTableUserQuery { Parameters = parameters }));
        }
    }
}
