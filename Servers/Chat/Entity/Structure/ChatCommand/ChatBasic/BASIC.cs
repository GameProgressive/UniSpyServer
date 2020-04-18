using System;
using Chat.Entity.Structure.Enumerator.Request;

namespace Chat.Entity.Structure.ChatCommand
{
    public class BASIC : ChatCommandBase
    {
        public BASIC()
        {
        }

        public BASIC(string request) : base(request)
        {
        }
    }
}
