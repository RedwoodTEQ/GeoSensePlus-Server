using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeoSensePlus.Mqtt
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
