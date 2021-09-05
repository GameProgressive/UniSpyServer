using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request.General
{
    internal sealed class GETUDPRELAYRequest : ChatChannelRequestBase
    {
        public GETUDPRELAYRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
