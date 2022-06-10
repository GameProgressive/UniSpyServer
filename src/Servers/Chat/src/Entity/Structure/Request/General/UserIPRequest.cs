using UniSpyServer.Servers.Chat.Abstraction.BaseClass;


namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
{
    
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
