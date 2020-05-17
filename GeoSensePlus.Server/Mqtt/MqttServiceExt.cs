using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoSensePlus.Server.Mqtt
{
    public static class MqttServiceExt
    {
        public static void AddMqtt(this IServiceCollection services)
        {
            services.AddSingleton<IMqttService, MqttService>();
            services.AddHostedService<MqttHostedService>();
        }
    }
}
