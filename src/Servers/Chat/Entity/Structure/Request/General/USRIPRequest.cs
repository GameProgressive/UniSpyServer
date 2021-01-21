using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request
{
    public class USRIPRequest : ChatRequestBase
    {
        public USRIPRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            if (!ErrorCode)
            {
                ErrorCode = false;
                return;
            }

            ErrorCode = true; // USRIP content is empty!
        }
    }
}
