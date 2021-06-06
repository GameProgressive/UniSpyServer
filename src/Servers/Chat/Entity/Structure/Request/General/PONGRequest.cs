using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request.General
{
    public class PONGRequest : ChatRequestBase
    {
        public PONGRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string EchoMessage { get; protected set; }

        public override void Parse()
        {
            base.Parse();


            EchoMessage = _longParam;
        }
    }
}
