namespace UniSpyLib.Abstraction.Interface
{
    public interface IUniSpyRedisChannelEvent
    {
        void StartSubscribe();
        void ReceivedMessage();
        void PublishMessage();
    }
}
