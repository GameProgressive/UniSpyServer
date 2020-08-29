using System;

namespace Chat.Entity.Structure.ChatCommand
{
    public class PRIVMSG : ChatMessagRequestBase
    {
        public PRIVMSG(string rawRequest) : base(rawRequest)
        {
        }
    }
}
