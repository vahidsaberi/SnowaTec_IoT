using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System.Net;

namespace SnowaTec.Test.RealTime
{
    public class MqttNetServer
    {
        private string _server = "127.0.0.1";
        private int _port = 5000;
        private string _username = "SnowaTec";
        private string _password = "Bo!2bjaq";
        private MqttServer? _mqttServer;

        public MqttNetServer() { }
        public MqttNetServer(string server, int port, string username, string password)
        {
            _server = server;
            _port = port;
            _username = username;
            _password = password;
        }

        public async void Start()
        {
            if (_mqttServer is not null)
            {
                return;
            }

            var optionBuilder = new MqttServerOptionsBuilder()
                .WithDefaultEndpoint()
                .WithConnectionBacklog(1000)
                .WithDefaultEndpointPort(_port)
                .WithDefaultEndpointBoundIPAddress(IPAddress.Parse(_server));

            var options = optionBuilder.Build();

            _mqttServer = new MqttFactory().CreateMqttServer(options);

            _mqttServer.ValidatingConnectionAsync += c =>
            {
                c.ReasonCode = MqttConnectReasonCode.Success;
                if (c.ClientId.Length < 10)
                {
                    c.ReasonCode = MqttConnectReasonCode.ClientIdentifierNotValid;
                }

                if (!c.UserName.Equals(_username))
                {
                    c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                }

                if (!c.Password.Equals(_password))
                {
                    c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                }

                return Task.CompletedTask;
            };

            _mqttServer.ClientConnectedAsync += e =>
            {
                Console.WriteLine($"{DateTime.Now} Client Connected:ClientId:{e.ClientId}");
                var s = _mqttServer.GetSessionsAsync();
                Console.WriteLine($@"{DateTime.Now} Total number of connections：{s.Result.Count}");
                return Task.CompletedTask;
            };

            _mqttServer.ClientDisconnectedAsync += e =>
            {
                Console.WriteLine($"{DateTime.Now} Client Disconnected:ClientId:{e.ClientId}");
                var s = _mqttServer.GetSessionsAsync();
                Console.WriteLine($@"{DateTime.Now} Total number of connections：{s.Result.Count}");
                return Task.CompletedTask;
            };

            _mqttServer.ClientSubscribedTopicAsync += e =>
            {
                Console.WriteLine($"{DateTime.Now} Client subscribed topic. ClientId:{e.ClientId} Topic:{e.TopicFilter.Topic} QualityOfServiceLevel:{e.TopicFilter.QualityOfServiceLevel}");
                return Task.CompletedTask;
            };

            _mqttServer.ClientUnsubscribedTopicAsync += e =>
            {
                Console.WriteLine($"{DateTime.Now} Client unsubscribed topic. ClientId:{e.ClientId} Topic:{e.TopicFilter.Length}");
                return Task.CompletedTask;
            };

            _mqttServer.StartedAsync += e =>
            {
                Console.WriteLine($"{DateTime.Now} Mqtt Server Started on ({_server}:{_port})...");
                return Task.CompletedTask;
            };

            _mqttServer.StoppedAsync += e =>
            {
                Console.WriteLine($"{DateTime.Now} Mqtt Server Stopped...");
                return Task.CompletedTask;
            };

            await _mqttServer.StartAsync();
        }

        public async void Stop()
        {
            if (_mqttServer is not null)
            {
                foreach (var clientSessionStatus in _mqttServer.GetSessionsAsync().Result)
                {
                    clientSessionStatus.DeleteAsync();
                }

                _mqttServer.StopAsync();
                _mqttServer = null;
            }
        }
    }
}
