using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Mqtt
{
    public interface IMqttClientService
    {
        Task ConnectAsync();
        Task SubscribeTopic(string topic);
    }

    public class MqttClientService : IMqttClientService
    {
        IMqttClient mqttClient;

        public async Task ConnectAsync()
        {
            // Create a new MQTT client.
            mqttClient = new MqttFactory().CreateMqttClient();

            // Create TCP based options using the builder.
            var options = new MqttClientOptionsBuilder().WithClientId("Client1")
                                                        .WithTcpServer("localhost", 11883)
                                                        //.WithWebSocketServer("broker.hivemq.com:8000/mqtt")
                                                        //.WithCredentials("bud", "%spencer%")
                                                        //.WithTls()
                                                        //.WithCleanSession()
                                                        .Build();

            // event handlers
            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();
                return Task.CompletedTask;
            };

            mqttClient.ConnectedAsync += e =>
            {
                Console.WriteLine("### CONNECTED WITH BROKER ###");
                return Task.CompletedTask;
            };

            // connect
            await mqttClient.ConnectAsync(options);
        }

        public async Task SubscribeTopic(string topic)
        {
            Console.WriteLine(mqttClient != null);
            //var subscribeResult = await mqttClient.SubscribeAsync(new TopicFilterBuilder()
            var subscribeResult = await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder()
               .WithTopic(topic)
               //.WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
               .Build());

            Console.WriteLine("### SUBSCRIBED ###");
            Console.WriteLine("### Result: " + subscribeResult.Items.FirstOrDefault()?.ResultCode);
            Console.WriteLine("### Result: " + subscribeResult.Items.FirstOrDefault()?.TopicFilter);
        }
    }
}
