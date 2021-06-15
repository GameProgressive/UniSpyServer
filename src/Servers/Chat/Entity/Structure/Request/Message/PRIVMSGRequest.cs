using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request
{
    internal sealed class PRIVMSGRequest : ChatMsgRequestBase
    {
        public PRIVMSGRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
