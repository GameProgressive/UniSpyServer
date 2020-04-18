using System;
namespace Chat.Entity.Structure.ChatCommand.ChatBasic
{
    public class PONG : ChatCommandBase
    {
       public string EchoMessage { get; protected set; }
        public PONG(string request) : base(request)
        {
        }
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
