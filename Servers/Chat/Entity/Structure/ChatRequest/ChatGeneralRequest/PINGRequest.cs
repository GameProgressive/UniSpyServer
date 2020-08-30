using System;
using Chat.Entity.Structure.ChatResponse;

namespace Chat.Entity.Structure.ChatCommand
{
    public class PINGRequest : ChatRequestBase
    {
        public PINGRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
