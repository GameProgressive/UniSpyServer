namespace UniSpyServer.UniSpyLib.Abstraction.Interface
{
    public interface IRedisChannelEvent
    {
        void StartSubscribe();
        void ReceivedMessage();
        void PublishMessage();
    }
}
