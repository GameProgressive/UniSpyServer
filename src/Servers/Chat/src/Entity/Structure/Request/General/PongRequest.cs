using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;

namespace Chat.Entity.Structure.Request.General
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


            EchoMessage = _longParam;
        }
    }
}
