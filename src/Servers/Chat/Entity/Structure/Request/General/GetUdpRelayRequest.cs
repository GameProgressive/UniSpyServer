using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;

namespace Chat.Entity.Structure.Request.General
{
    [RequestContract("GETUDPREPLAY")]
    internal sealed class GetUdpRelayRequest : ChannelRequestBase
    {
        public GetUdpRelayRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
