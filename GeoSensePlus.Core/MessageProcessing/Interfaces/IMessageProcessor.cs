namespace GeoSensePlus.Core.MessageProcessing.Interfaces
{
    // Separate process for easier unit testing
    public interface IMessageProcessor<T>
    {
        void Process(T message);
    }
}
