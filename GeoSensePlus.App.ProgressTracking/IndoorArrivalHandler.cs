using GeoSensePlus.App.ProgressTracking.Messages;
using GeoSensePlus.Core.Codec;
using GeoSensePlus.Core.MessageProcessing;
using GeoSensePlus.Core.MessageProcessing.BaseHandlers;
using GeoSensePlus.Core.MessageProcessing.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Core.CommandProcessing.MessageHandlers
{
    public class IndoorArrivalHandler : JsonHandler<IndoorArrivalMessage>
    {
        public IndoorArrivalHandler(IMessageProcessor<IndoorArrivalMessage> processor) : base(processor)
        {
        }

        protected override IndoorArrivalMessage Parse(dynamic msg)
        {
            return new IndoorArrivalMessage
            {
                TimeStamp = msg.timestamp,
                UserDeviceId = msg.user.id,
                UserDeviceBatteryLevel = msg.user.props.battery,
                UserDeviceType = msg.user.props.device_type,

                TagId = msg.tag.id,
                TagSignal = msg.tag.props.signal,
                TagBatteryLevel = msg.tag.props.battery
            };
        }

        //protected override void Execute(IndoorArrivalMessage data, ChannelContext<string> ctx)
        //{
        //    Console.WriteLine("\nHandling IndoorArrivalMessage message ...");
        //    Console.WriteLine($"ReceiveTime = {ctx.ReceiveTime}; UserDeviceId = {data.UserDeviceId}; TagId = {data.TagId}");
        //}
    }
}
