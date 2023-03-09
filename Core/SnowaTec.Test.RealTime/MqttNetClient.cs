using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using System.Text;

namespace SnowaTec.Test.RealTime
{
    public class MqttNetClient
    {
        private string _server = "127.0.0.1";
        private int _port = 5000;
        private string _username = "SnowaTec";
        private string _password = "Bo!2bjaq";

        private IMqttClient _mqttClient;

        private readonly List<IManagedMqttClient> _managedMqttClients = new();

        private Func<string, dynamic> ReceiveMessage;

        public MqttNetClient() { }
        public MqttNetClient(string server, int port, string username, string password)
        {
            _server = server;
            _port = port;
            _username = username;
            _password = password;
        }

        public void Initial(Func<string, dynamic> receiveMessage) { ReceiveMessage = receiveMessage; }

        public async Task<string> Connect(string clientId)
        {
            try
            {
                var options = new MqttClientOptions
                {
                    ClientId = clientId,
                    ProtocolVersion = MqttProtocolVersion.V500,
                };

                options.ChannelOptions = new MqttClientTcpOptions
                {
                    Server = _server,
                    Port = _port
                };

                options.Credentials = new MqttClientCredentials(_username, Encoding.UTF8.GetBytes(_password));

                options.CleanSession = true;
                options.KeepAlivePeriod = TimeSpan.FromSeconds(100.5);

                if (null != _mqttClient)
                {
                    await _mqttClient.DisconnectAsync();
                    _mqttClient = null;
                }

                _mqttClient = new MqttFactory().CreateMqttClient();

                _mqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    if (ReceiveMessage != null)
                    {
                        ReceiveMessage(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
                    }

                    return Task.FromResult($"{DateTime.Now} ClientID:{e.ClientId} | TOPIC:{e.ApplicationMessage.Topic} | Payload:{Encoding.UTF8.GetString(e.ApplicationMessage.Payload)} | QoS:{e.ApplicationMessage.QualityOfServiceLevel} | Retain:{e.ApplicationMessage.Retain}");
                };

                _mqttClient.DisconnectedAsync += e =>
                {
                    return Task.FromResult($"{DateTime.Now} Client is Connected:  IsSessionPresent:{e.ConnectResult}");
                };

                await _mqttClient.ConnectAsync(options, CancellationToken.None);

                return "Creating a client and connection was successfully...";
            }
            catch (Exception ex)
            {
                return $"Failed to connect {ex.Message}";
            }
        }

        public async Task<string> MultiConnect(int clientsCount, string topic, MqttQualityOfServiceLevel quality)
        {
            try
            {
                for (int i = 0; i < clientsCount; i++)
                {
                    var options = new ManagedMqttClientOptionsBuilder()
                        .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                        .WithClientOptions(new MqttClientOptionsBuilder()
                        .WithClientId(Guid.NewGuid().ToString().Substring(0, 13))
                        .WithTcpServer(_server, _port)
                        .WithCredentials(_username, _password)
                        .Build()
                    )
                    .Build();

                    IManagedMqttClient c = new MqttFactory().CreateManagedMqttClient();

                    var so = new MqttClientSubscribeOptions();

                    so.TopicFilters = new List<MqttTopicFilter>
                    {
                        new MqttTopicFilter
                        {
                            Topic = topic,
                            QualityOfServiceLevel = (MqttQualityOfServiceLevel)Enum.Parse(typeof(MqttQualityOfServiceLevel), quality.ToString())
                        }
                    };

                    await c.SubscribeAsync(so.TopicFilters);

                    await c.StartAsync(options);

                    _managedMqttClients.Add(c);

                    Thread.Sleep(200);
                }

                return "Creating clients and connection was successfully...";
            }
            catch (Exception ex)
            {
                return $"Failed to connect {ex.Message}";
            }
        }

        public async Task<string> Disconnect(string clientId)
        {
            try
            {
                if (_mqttClient is not null && _mqttClient.IsConnected)
                {
                    await _mqttClient.DisconnectAsync();
                    _mqttClient.Dispose();
                    _mqttClient = null;
                }

                return "Disconnect client was successfully...";
            }
            catch (Exception ex)
            {
                return $"Failed to disconnect {ex.Message}";
            }
        }

        public async Task<string> MultiDisconnect(string clientId)
        {
            try
            {
                foreach (IManagedMqttClient client in _managedMqttClients)
                {
                    await client.StopAsync();
                    client.Dispose();
                    Thread.Sleep(100);
                }

                return "Disconnect clients was successfully...";
            }
            catch (Exception ex)
            {
                return $"Failed to disconnect {ex.Message}";
            }
        }

        public async Task<string> Subscribe(string topic, MqttQualityOfServiceLevel quality)
        {
            try
            {
                var so = new MqttClientSubscribeOptions();

                so.TopicFilters = new List<MqttTopicFilter>
                {
                    new MqttTopicFilter
                    {
                        Topic = topic,
                        QualityOfServiceLevel = (MqttQualityOfServiceLevel)Enum.Parse(typeof(MqttQualityOfServiceLevel), quality.ToString())
                    }
                };

                await _mqttClient.SubscribeAsync(so);

                return "Subscribe was successfully...";
            }
            catch (Exception ex)
            {
                return $"Failed to Subscribe {ex.Message}";
            }
        }

        public async Task<string> Send(string topic, string message, MqttQualityOfServiceLevel quality)
        {
            try
            {
                var msg = new MqttApplicationMessage
                {
                    Topic = topic,
                    Payload = Encoding.UTF8.GetBytes(message),
                    QualityOfServiceLevel = (MqttQualityOfServiceLevel)Enum.Parse(typeof(MqttQualityOfServiceLevel), quality.ToString()),
                    Retain = false
                };

                if (_mqttClient is not null)
                {
                    await _mqttClient.PublishAsync(msg);
                }
                return "Send was successfully...";
            }
            catch (Exception ex)
            {
                return $"Failed to Send {ex.Message}";
            }
        }
    }
}
