using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.Message
{
    [RequestContract("PRIVMSG")]
    public sealed class PrivateMsgRequest : MsgRequestBase
    {
        public PrivateMsgRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
