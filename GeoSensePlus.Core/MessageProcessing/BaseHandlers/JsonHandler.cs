using GeoSensePlus.Core.Codec;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GeoSensePlus.Core.MessageProcessing.BaseHandlers
{
    public abstract class JsonHandler<TMessage> : IMessageHandler<string>
    {
        public bool Handle(string jsonString, ChannelContext<string> ctx)
        {
            try
            {
                ctx.RawMessage = jsonString;

                // parse json with local time zone settings
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString, new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Local
                });

                TMessage message = this.Parse(jsonObject);
                if (message != null)
                {
                    ctx.MessageObject = message;
                    this.Execute(message, ctx);
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.OnError(ex);
            }

            return false;
        }

        protected abstract TMessage Parse(dynamic msg);
        protected abstract void Execute(TMessage message, ChannelContext<string> ctx);
        protected virtual void OnError(Exception ex) { }
    }
}
