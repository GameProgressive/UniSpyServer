using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class GETUDPRELAYRequest : ChatChannelRequestBase
    {
        public GETUDPRELAYRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
