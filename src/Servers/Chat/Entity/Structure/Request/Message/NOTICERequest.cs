using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request
{
    internal sealed class NOTICERequest : ChatMsgRequestBase
    {
        public NOTICERequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
