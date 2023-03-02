using UniSpy.Server.Chat.Abstraction.BaseClass;

namespace UniSpy.Server.Chat.Aggregate.Redis.Contract
{
    public class DisconnectRequest : RequestBase
    {
        public DisconnectRequest() : base("DISCONNECT")
        {
            Parse();
        }
    }
}