using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class USRIPRequest : ChatRequestBase
    {
        public USRIPRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override object Parse()
        {
            if(!(bool)base.Parse())
            {
                return false;
            }

            return true; // USRIP content is empty!
        }
    }
}
