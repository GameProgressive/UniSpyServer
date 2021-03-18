using System;
namespace UniSpyLib.Abstraction.Interface
{
    public interface IUniSpyRedisChannelEvent
    {
        public void StartSubscribe();
        public void ReceivedMessage();
        public void PublishMessage();
    }
}
