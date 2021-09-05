using Chat.Abstraction.BaseClass;
using Chat.Entity.Exception;

namespace Chat.Entity.Structure.Request.General
{
    internal sealed class QUITRequest : ChatRequestBase
    {
        public QUITRequest() { }
        public QUITRequest(string rawRequest) : base(rawRequest)
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
