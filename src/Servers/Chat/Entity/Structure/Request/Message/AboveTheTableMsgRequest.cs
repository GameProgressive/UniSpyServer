using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;

namespace Chat.Entity.Structure.Request.Message
{
    [RequestContract("ATM")]
    internal sealed class AboveTheTableMsgRequest : ChatMsgRequestBase
    {
        public AboveTheTableMsgRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
