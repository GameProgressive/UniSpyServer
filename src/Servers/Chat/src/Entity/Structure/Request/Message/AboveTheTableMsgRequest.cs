using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;

namespace UniSpyServer.Chat.Entity.Structure.Request.Message
{
    [RequestContract("ATM")]
    public sealed class AboveTheTableMsgRequest : MsgRequestBase
    {
        public AboveTheTableMsgRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
