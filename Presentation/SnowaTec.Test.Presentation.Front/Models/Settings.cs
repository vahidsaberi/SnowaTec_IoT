namespace SnowaTec.Test.Presentation.Front.Models
{
    public class Settings
    {
        public string MqttWebSocket { get; set; }
        public string ApiHost { get; set; }
        public Settings(IConfiguration configuration)
        {
            var settings = configuration.GetSection("Settings");
            this.MqttWebSocket = settings.GetValue<string>("MqttWebSocket");
            this.ApiHost = settings.GetValue<string>("ApiHost");
        }
    }
}
