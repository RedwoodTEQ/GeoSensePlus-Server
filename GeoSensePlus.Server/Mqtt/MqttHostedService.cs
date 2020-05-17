using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client.Receiving;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeoSensePlus.Server.Mqtt
{
    public class MqttHostedService : IHostedService, IDisposable
    {
        private readonly IMqttService _mqttService;

        public MqttHostedService(IMqttService mqttService)
        {
            _mqttService = mqttService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _mqttService.StartAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _mqttService.StopAsync();
        }

        public void Dispose()
        {
            Console.WriteLine("MQTT service terminated.");
        }
    }
}
