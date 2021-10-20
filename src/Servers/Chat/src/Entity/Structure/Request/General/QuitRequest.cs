using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Exception;

namespace UniSpyServer.Chat.Entity.Structure.Request.General
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
                throw new Exception.Exception("Quit reason is missing.");
            }

            Reason = _longParam;
        }
    }
}
