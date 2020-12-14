using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
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
            if(!ErrorCode)
            {
               ErrorCode = false;
                return;
            }

            EchoMessage = _longParam;
            ErrorCode = true;
        }
    }
}
