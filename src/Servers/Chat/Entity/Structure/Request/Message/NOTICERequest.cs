using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class NOTICERequest : ChatMsgRequestBase
    {
        public NOTICERequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
