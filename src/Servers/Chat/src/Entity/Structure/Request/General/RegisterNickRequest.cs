using UniSpyServer.Servers.Chat.Abstraction.BaseClass;


namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
{
    
    public sealed class RegisterNickRequest : RequestBase
    {
        public RegisterNickRequest(string rawRequest) : base(rawRequest){ }

        public string NamespaceID { get; private set; }
        public string UniqueNick { get; private set; }
        public string CDKey { get; private set; }
        public override void Parse()
        {
            base.Parse();

            NamespaceID = _cmdParams[0];
            UniqueNick = _cmdParams[1];
            CDKey = _cmdParams[2];
        }
    }
}
