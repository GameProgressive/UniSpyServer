using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.Message
{
    [RequestContract("UTM")]
    public sealed class UnderTheTableMsgRequest : MsgRequestBase
    {
        public UnderTheTableMsgRequest(string rawRequest) : base(rawRequest){ }
    }
}
