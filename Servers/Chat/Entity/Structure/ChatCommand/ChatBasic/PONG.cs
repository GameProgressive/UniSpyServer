using System;
namespace Chat.Entity.Structure.ChatCommand
{
    public class PONG : ChatCommandBase
    {
       public string EchoMessage { get; protected set; }

        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            {
                return false;
            }
            EchoMessage = _longParam;
            return true;
        }
    }
}
