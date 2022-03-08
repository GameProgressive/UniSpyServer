using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
{
    [RequestContract("USRIP")]
    public sealed class UserIPRequest : RequestBase
    {
        public UserIPRequest(string rawRequest) : base(rawRequest){ }

        public override void Parse()
        {
            base.Parse();
            // USRIP content is empty!
        }
    }
}
