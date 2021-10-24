using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;

namespace UniSpyServer.Chat.Entity.Structure.Request.Message
{
    [RequestContract("NOTICE")]
    public sealed class NoticeRequest : MsgRequestBase
    {
        public NoticeRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
