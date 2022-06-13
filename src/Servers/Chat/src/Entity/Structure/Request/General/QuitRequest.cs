using UniSpyServer.Servers.Chat.Abstraction.BaseClass;


namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
{
    
    public sealed class QuitRequest : RequestBase
    {
        public QuitRequest() { }
        public QuitRequest(string rawRequest) : base(rawRequest){ }

        public string Reason { get; set; }

        public override void Parse()
        {
            base.Parse();

            if (_longParam is null)
            {
                throw new Exception.ChatException("Quit reason is missing.");
            }

            Reason = _longParam;
        }
    }
}
