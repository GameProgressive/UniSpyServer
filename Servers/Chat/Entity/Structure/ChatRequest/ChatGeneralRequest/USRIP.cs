using Chat.Entity.Structure.ChatResponse;

namespace Chat.Entity.Structure.ChatCommand
{
    public class USRIP : ChatRequestBase
    {
        public USRIP(string rawRequest) : base(rawRequest)
        {
        }

        public override bool Parse()
        {
            return true; // USRIP content is empty!
        }
    }
}
