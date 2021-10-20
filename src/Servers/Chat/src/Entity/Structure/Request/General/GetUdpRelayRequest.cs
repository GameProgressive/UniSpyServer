using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;

namespace UniSpyServer.Chat.Entity.Structure.Request.General
{
    [RequestContract("GETUDPREPLAY")]
    public sealed class GetUdpRelayRequest : ChannelRequestBase
    {
        public GetUdpRelayRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
