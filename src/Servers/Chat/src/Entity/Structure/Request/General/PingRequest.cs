using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;

namespace UniSpyServer.Chat.Entity.Structure.Request.General
{
    [RequestContract("PING")]
    public sealed class PingRequest : RequestBase
    {
        public PingRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
