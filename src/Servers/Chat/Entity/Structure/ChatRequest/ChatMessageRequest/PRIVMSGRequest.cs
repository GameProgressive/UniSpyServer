using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class PRIVMSGRequest : ChatMessagRequestBase
    {
        public PRIVMSGRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
