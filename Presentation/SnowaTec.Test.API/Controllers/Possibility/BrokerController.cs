using Microsoft.AspNetCore.Mvc;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.RealTime;
using System.Threading.Tasks;

namespace SnowaTec.Test.API.Controllers.Possibility
{
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "WebAPISpecification")]
    public class BrokerController : ControllerBase
    {
        private MqttNetClient _mqttClient;

        public BrokerController(MqttNetClient mqttClient)
        {
            _mqttClient = mqttClient;
        }

        [HttpPost(ApiRoutes.Brokers.Connect)]
        public async Task<IActionResult> Connect(string clientId)
        {
            try
            {
                return Ok(await _mqttClient.Connect(clientId));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost(ApiRoutes.Brokers.Disconnect)]
        public async Task<IActionResult> Disconnect()
        {
            try
            {
                return Ok(await _mqttClient.Disconnect(""));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost(ApiRoutes.Brokers.Subscribe)]
        public async Task<IActionResult> Subscribe(string topic)
        {
            try
            {
                return Ok(await _mqttClient.Subscribe(topic, MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost(ApiRoutes.Brokers.Send)]
        public async Task<IActionResult> Send(string topic, string message)
        {
            try
            {
                return Ok(await _mqttClient.Send(topic, message, MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
