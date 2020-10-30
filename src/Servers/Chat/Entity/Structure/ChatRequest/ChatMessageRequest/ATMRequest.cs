using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class ATMRequest : ChatMessagRequestBase
    {
        public ATMRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
