using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
{
    [RequestContract("GETUDPREPLAY")]
    public sealed class GetUdpRelayRequest : ChannelRequestBase
    {
        public GetUdpRelayRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
