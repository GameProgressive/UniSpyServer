using UniSpyServer.Servers.Chat.Abstraction.BaseClass;


namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
{
    
    public sealed class CDKeyRequest : RequestBase
    {

        public string CDKey { get; private set; }
        public CDKeyRequest(string rawRequest) : base(rawRequest) { }
        public override void Parse()
        {
            base.Parse();

            if (_cmdParams.Count < 1)
            {
                throw new Exception.ChatException("The number of IRC cmdParams are incorrect.");
            }

            CDKey = _cmdParams[0];
        }
    }
}
