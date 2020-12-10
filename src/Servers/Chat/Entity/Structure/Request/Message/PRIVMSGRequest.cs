using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class PRIVMSGRequest : ChatMsgRequestBase
    {
        public PRIVMSGRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
