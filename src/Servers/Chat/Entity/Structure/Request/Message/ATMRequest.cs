using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand.Message
{
    public class ATMRequest : ChatMessagRequestBase
    {
        public ATMRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
