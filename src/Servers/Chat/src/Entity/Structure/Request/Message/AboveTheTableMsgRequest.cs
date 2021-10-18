using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;

namespace Chat.Entity.Structure.Request.Message
{
    [RequestContract("ATM")]
    public sealed class AboveTheTableMsgRequest : MsgRequestBase
    {
        public AboveTheTableMsgRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
