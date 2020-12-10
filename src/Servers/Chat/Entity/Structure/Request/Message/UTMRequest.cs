using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class UTMRequest : ChatMsgRequestBase
    {
        public UTMRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
