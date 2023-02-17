namespace UniSpy.Server.Core.Abstraction.Interface
{
    public interface IRedisChannelEvent
    {
        void StartSubscribe();
        void ReceivedMessage();
        void PublishMessage();
    }
}
