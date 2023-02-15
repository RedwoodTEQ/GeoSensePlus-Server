using GeoSensePlus.App.AssetTracking.Codecs;
using GeoSensePlus.App.AssetTracking.Handlers;
using GeoSensePlus.App.AssetTracking.MessageProcessors;
using GeoSensePlus.App.AssetTracking.Messages;
using GeoSensePlus.Core.Codec;
using GeoSensePlus.Core.MessageProcessing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace GeoSensePlus.App.AssetTracking
{
    static public class Extensions
    {
        static public void AddAssetTracking(this IServiceCollection services)
        {
            services.AddTransient<IMessageHandler<string>, IndoorAssetReportHandler>();
            services.AddTransient<IPayloadDecoder<List<IndoorTagPayloadInfo>>, IndoorTagPayloadDecoder>();
            services.AddTransient<IPayloadEncoder<List<IndoorTagPayloadInfo>>, IndoorTagPayloadEncoder>();
            services.AddTransient<IMessageExecutor<IndoorAssetReportMessage>, IndoorAssetReportProcessor>();
        }
    }
}
