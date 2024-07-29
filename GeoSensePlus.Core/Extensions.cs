using GeoSensePlus.Core.MessageProcessing;
using GeoSensePlus.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GeoSensePlus.Core
{
    static public class Extensions
    {
        static public void AddGeoSensePlusCore(this IServiceCollection services)
        {
            //services.AddTransient(typeof(IMessageProcessor<>), typeof(MessageProcessor<>));
            services.AddTransient<IMessageEngine, MessageEngine>();
            services.AddTransient<IDeviceService, DeviceService>();
        }
    }
}
