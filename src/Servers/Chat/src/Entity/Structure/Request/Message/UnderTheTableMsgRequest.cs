using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;

namespace UniSpyServer.Chat.Entity.Structure.Request.Message
{
    [RequestContract("UTM")]
    public sealed class UnderTheTableMsgRequest : MsgRequestBase
    {
        public UnderTheTableMsgRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
