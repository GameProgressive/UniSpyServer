using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;

namespace Chat.Entity.Structure.Request
{
    [RequestContract("PRIVMSG")]
    internal sealed class PrivateMsgRequest : MsgRequestBase
    {
        public PrivateMsgRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
