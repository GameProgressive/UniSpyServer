using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;

namespace UniSpyServer.Chat.Entity.Structure.Request.General
{
    [RequestContract("PONG")]
    public sealed class PongRequest : RequestBase
    {
        public PongRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string EchoMessage { get; private set; }

        public override void Parse()
        {
            base.Parse();

            if (_longParam == null)
            {
                throw new Exception.Exception("Echo message is missing.");
            }

            EchoMessage = _longParam;
        }
    }
}
