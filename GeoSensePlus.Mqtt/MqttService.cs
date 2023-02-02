using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Server;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeoSensePlus.Mqtt
{
    public interface IMqttService
    {
        int Port { get; }
        Task StartAsync();
        Task StopAsync();
        Task PublishAsync(string topic, string message);
    }

    public class MqttService : IMqttService
    {
        private const int _port = 11883;
        private MqttServer _mqttServer;
        private readonly ILogger _logger;

        public MqttService(ILogger<MqttHostedService> logger)
        {
            _logger = logger;
        }

        public int Port => _port;

        private void InitEventHandlers(MqttServer svr)
        {
            svr.InterceptingPublishAsync += e => {
                var clientId = e.ClientId;
                var topic = e.ApplicationMessage.Topic;
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                _logger.LogInformation($"MQTT client [{clientId}] published to topic [{topic}] with payload: [{payload}]");
                return Task.CompletedTask;
            };

            svr.ClientConnectedAsync += e => {
                _logger.LogInformation(e.ClientId + " Connected.");
                return Task.CompletedTask;
            };

            svr.ClientDisconnectedAsync += e=> { 
                _logger.LogInformation(e.ClientId + " Disonnected.");
                return Task.CompletedTask;
            };

            svr.ClientSubscribedTopicAsync += e => {
                _logger.LogInformation(e.ClientId + " subscribed to " + e.TopicFilter);
                return Task.CompletedTask;
            };

            svr.ClientUnsubscribedTopicAsync += e => { 
                _logger.LogInformation(e.ClientId + " unsubscribed to " + e.TopicFilter);
                return Task.CompletedTask;
            };
        }

        public Task StartAsync()
        {
            // The port for the default endpoint is 1883.
            // The default endpoint is NOT encrypted!
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(1000)
                .WithDefaultEndpoint()
                .WithDefaultEndpointPort(_port)
                .Build();


            _mqttServer = new MqttFactory().CreateMqttServer(optionsBuilder);
            InitEventHandlers(_mqttServer);

            Console.WriteLine($"MQTT service (over TCP) listening on port {_port}");
            return _mqttServer.StartAsync();
        }

        public async Task StopAsync()
        {
            await _mqttServer.StopAsync();
            _mqttServer.Dispose();
        }

        public async Task PublishAsync(string topic, string message)
        {
            var mqttMsg = new MqttApplicationMessageBuilder().WithTopic(topic)
                                                             .WithPayload(message)
                                                             .Build();
            // send message
            await _mqttServer.InjectApplicationMessage(
                new InjectedMqttApplicationMessage(mqttMsg)
                {
                    SenderClientId = "server"
                });
        }
    }
}