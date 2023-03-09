using Microsoft.AspNetCore.Mvc;
using SnowaTec.Test.Domain.Settings;
using SnowaTec.Test.Service.Contract.Possibility;
using System.Threading.Tasks;

namespace SnowaTec.Test.API.Controllers.Possibility
{
    [ApiController]
    [Route("api/v{version:apiVersion}/Mail")]
    [ApiExplorerSettings(GroupName = "WebAPISpecification")]
    public class MailController : ControllerBase
    {
        private readonly IEmailService mailService;
        public MailController(IEmailService mailService)
        {
            this.mailService = mailService;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        {
            await mailService.SendEmailAsync(request);
            return Ok();
        }

    }
}