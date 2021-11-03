using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.Message
{
    [RequestContract("NOTICE")]
    public sealed class NoticeRequest : MsgRequestBase
    {
        public NoticeRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
