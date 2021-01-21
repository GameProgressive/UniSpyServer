using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request.Message
{
    public class ATMRequest : ChatMsgRequestBase
    {
        public ATMRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
