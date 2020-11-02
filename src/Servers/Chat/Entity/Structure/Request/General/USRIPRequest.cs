using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class USRIPRequest : ChatRequestBase
    {
        public USRIPRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override bool Parse()
        {
            if (!base.Parse())
            {
                return false;
            }

            return true; // USRIP content is empty!
        }
    }
}
