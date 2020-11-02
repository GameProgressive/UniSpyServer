using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class UTMRequest : ChatMessagRequestBase
    {
        public UTMRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
