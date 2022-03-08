using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
{
    [RequestContract("PONG")]
    public sealed class PongRequest : RequestBase
    {
        public PongRequest(string rawRequest) : base(rawRequest){ }

        public string EchoMessage { get; private set; }

        public override void Parse()
        {
            base.Parse();

            if (_longParam == null)
            {
                throw new Exception.ChatException("Echo message is missing.");
            }
            EchoMessage = _longParam;
        }
    }
}
