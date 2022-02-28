using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
{
    [RequestContract("QUIT")]
    public sealed class QuitRequest : RequestBase
    {
        public QuitRequest() { }
        public QuitRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string Reason { get; set; }

        public override void Parse()
        {
            base.Parse();

            if (_longParam == null)
            {
                throw new Exception.ChatException("Quit reason is missing.");
            }

            Reason = _longParam;
        }
    }
}
