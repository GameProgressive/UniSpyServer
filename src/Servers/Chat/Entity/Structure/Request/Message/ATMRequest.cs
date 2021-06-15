using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request.Message
{
    internal sealed class ATMRequest : ChatMsgRequestBase
    {
        public ATMRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
