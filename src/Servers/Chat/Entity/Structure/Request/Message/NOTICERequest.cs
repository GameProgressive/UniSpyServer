using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class NOTICERequest : ChatMessagRequestBase
    {
        public NOTICERequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
