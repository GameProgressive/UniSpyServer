using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand.Message
{
    public class ATMRequest : ChatMsgRequestBase
    {
        public ATMRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
