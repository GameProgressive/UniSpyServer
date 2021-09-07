using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Exception;

namespace Chat.Entity.Structure.Request.General
{
    [RequestContract("QUIT")]
    internal sealed class QuitRequest : RequestBase
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
                throw new ChatException("Quit reason is missing.");
            }

            Reason = _longParam;
        }
    }
}
