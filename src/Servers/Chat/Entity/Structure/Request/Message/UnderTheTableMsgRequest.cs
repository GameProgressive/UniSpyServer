using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;

namespace Chat.Entity.Structure.Request
{
    [RequestContract("UTM")]
    internal sealed class UnderTheTableMsgRequest : ChatMsgRequestBase
    {
        public UnderTheTableMsgRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
