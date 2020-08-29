using System;
namespace Chat.Entity.Structure.ChatCommand
{
    public class PONG : ChatRequestBase
    {
        public PONG(string rawRequest) : base(rawRequest)
        {
        }

        public string EchoMessage { get; protected set; }

        public override bool Parse()
        {
            if (!Parse())
            {
                return false;
            }
            EchoMessage = _longParam;
            return true;
        }
    }
}
