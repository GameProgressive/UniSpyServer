using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request
{
    internal sealed class UTMRequest : ChatMsgRequestBase
    {
        public UTMRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
