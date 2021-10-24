using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;

namespace UniSpyServer.Chat.Entity.Structure.Request.Message
{
    [RequestContract("PRIVMSG")]
    public sealed class PrivateMsgRequest : MsgRequestBase
    {
        public PrivateMsgRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
