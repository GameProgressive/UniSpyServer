using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.Message
{
    [RequestContract("ATM")]
    public sealed class AboveTheTableMsgRequest : MsgRequestBase
    {
        public AboveTheTableMsgRequest(string rawRequest) : base(rawRequest){ }
    }
}
