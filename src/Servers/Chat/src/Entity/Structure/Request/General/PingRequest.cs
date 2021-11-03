using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
{
    [RequestContract("PING")]
    public sealed class PingRequest : RequestBase
    {
        public PingRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
