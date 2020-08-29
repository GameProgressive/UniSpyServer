using System;
using Chat.Entity.Structure.ChatResponse;

namespace Chat.Entity.Structure.ChatCommand
{
    public class PING : ChatRequestBase
    {
        public PING(string rawRequest) : base(rawRequest)
        {
        }
    }
}
