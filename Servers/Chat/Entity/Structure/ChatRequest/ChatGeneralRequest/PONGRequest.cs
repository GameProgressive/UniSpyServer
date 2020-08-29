using System;
namespace Chat.Entity.Structure.ChatCommand
{
    public class PONGRequest : ChatRequestBase
    {
        public PONGRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string EchoMessage { get; protected set; }

        public override bool Parse()
        {
            if (!base.Parse())
            {
                return false;
            }
            EchoMessage = _longParam;
            return true;
        }
    }
}
