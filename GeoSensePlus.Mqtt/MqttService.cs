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
        private IMqttServer _mqttServer;
        private readonly ILogger _logger;

        public MqttService(ILogger<MqttHostedService> logger)
        {
            _logger = logger;
        }

        public int Port => _port;

        private void InitEventHandlers(IMqttServer svr)
        {
            svr.UseApplicationMessageReceivedHandler(e =>
            {
                var clientId = e.ClientId;
                var topic = e.ApplicationMessage.Topic;
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                _logger.LogInformation($"MQTT client [{e.ClientId}] published to topic [{topic}] with payload: [{payload}]");
            });

            svr.UseClientConnectedHandler(e =>
            {
                _logger.LogInformation(e.ClientId + " Connected.");
            });

            svr.UseClientDisconnectedHandler(e =>
            {
                _logger.LogInformation(e.ClientId + " Disonnected.");
            });

            svr.ClientSubscribedTopicHandler = new MqttServerClientSubscribedHandlerDelegate(e =>
            {
                _logger.LogInformation(e.ClientId + " subscribed to " + e.TopicFilter);
            });

            svr.ClientUnsubscribedTopicHandler = new MqttServerClientUnsubscribedTopicHandlerDelegate(e =>
            {
                _logger.LogInformation(e.ClientId + " unsubscribed to " + e.TopicFilter);
            });
        }

        public Task StartAsync()
        {
            var optionsBuilder = new MqttServerOptionsBuilder().WithConnectionBacklog(1000)
                                                               .WithDefaultEndpointPort(_port);

            _mqttServer = new MqttFactory().CreateMqttServer();
            InitEventHandlers(_mqttServer);

            Console.WriteLine($"MQTT service (over TCP) listening on port {_port}");
            return _mqttServer.StartAsync(optionsBuilder.Build());
        }

        public Task StopAsync()
        {
            return _mqttServer.StopAsync();
        }

        public async Task PublishAsync(string topic, string message)
        {
            var mqttMsg = new MqttApplicationMessageBuilder().WithTopic(topic)
                                                             .WithPayload(message)
                                                             .WithExactlyOnceQoS()
                                                             .WithRetainFlag()
                                                             .Build();
            await _mqttServer.PublishAsync(mqttMsg, CancellationToken.None);
        }
    }
}