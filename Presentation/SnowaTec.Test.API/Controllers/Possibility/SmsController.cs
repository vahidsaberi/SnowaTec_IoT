using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SnowaTec.Test.Domain.Settings;
using SnowaTec.Test.Service.Contract.Possibility;

namespace SnowaTec.Test.API.Controllers.Possibility
{
    [ApiController]
    [Route("api/v{version:apiVersion}/sms")]
    [ApiExplorerSettings(GroupName = "WebAPISpecification")]
    public class SmsController : ControllerBase
    {
        private readonly ISmsService smsService;
        public SmsController(ISmsService smsService)
        {
            this.smsService = smsService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] SmsRequest request)
        {
            await smsService.SendSMSAsync(request);
            return Ok();
        }
    }
}
