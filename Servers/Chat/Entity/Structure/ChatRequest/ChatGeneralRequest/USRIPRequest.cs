using Chat.Entity.Structure.ChatResponse;

namespace Chat.Entity.Structure.ChatCommand
{
    public class USRIPRequest : ChatRequestBase
    {
        public USRIPRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override bool Parse()
        {
            return true; // USRIP content is empty!
        }
    }
}
