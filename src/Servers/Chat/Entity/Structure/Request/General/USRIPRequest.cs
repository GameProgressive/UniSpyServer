using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request.General
{
    public class USRIPRequest : ChatRequestBase
    {
        public USRIPRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();


            // USRIP content is empty!
        }
    }
}
