using GeoSensePlus.App.ProgressTracking.Messages;
using GeoSensePlus.Core.MessageProcessing.Interfaces;
using System;

namespace GeoSensePlus.Core.CommandProcessing.MessageHandlers
{
    public class IndoorArrivalMessageProcessor : IMessageProcessor<IndoorArrivalMessage>
    {
        public void Process(IndoorArrivalMessage message)
        {
            Console.WriteLine("\nHandling IndoorArrivalMessage message ...");
            throw new NotImplementedException();
        }
    }
}
