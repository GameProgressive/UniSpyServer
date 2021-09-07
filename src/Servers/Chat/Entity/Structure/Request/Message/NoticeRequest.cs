using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;

namespace Chat.Entity.Structure.Request
{
    [RequestContract("NOTICE")]
    internal sealed class NoticeRequest : ChatMsgRequestBase
    {
        public NoticeRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
