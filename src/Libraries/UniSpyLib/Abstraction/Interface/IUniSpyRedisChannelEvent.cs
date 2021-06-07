using System;
namespace UniSpyLib.Abstraction.Interface
{
    interface IUniSpyRedisChannelEvent
    {
        void StartSubscribe();
        void ReceivedMessage();
        void PublishMessage();
    }
}
