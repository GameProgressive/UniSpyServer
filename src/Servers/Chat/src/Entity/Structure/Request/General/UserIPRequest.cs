using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;

namespace Chat.Entity.Structure.Request.General
{
    [RequestContract("USRIP")]
    public sealed class UserIPRequest : RequestBase
    {
        public UserIPRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();


            // USRIP content is empty!
        }
    }
}
