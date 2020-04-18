using System;
using Chat.Entity.Structure.Enumerator.Request;

namespace Chat.Entity.Structure.ChatCommand
{
    public class USRIP : ChatCommandBase
    {
        /// <summary>
        /// USRIP command only have a command name
        /// </summary>
        /// <param name="request"></param>
        public USRIP(string request) : base(request)
        {
        }

        public USRIP() : base()
        {
        }
    }
}
