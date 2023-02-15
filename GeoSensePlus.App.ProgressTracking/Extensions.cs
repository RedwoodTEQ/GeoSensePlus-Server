using GeoSensePlus.App.ProgressTracking.Messages;
using GeoSensePlus.Core.CommandProcessing.MessageHandlers;
using GeoSensePlus.Core.MessageProcessing.BaseHandlers;
using GeoSensePlus.Core.MessageProcessing.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GeoSensePlus.App.ProgressTracking
{
    static public class Extensions
    {
        static public void AddProgressTracking(this IServiceCollection services)
        {
            services.AddTransient<IMessageHandler<string>, IndoorArrivalHandler>();
        }
    }
}
