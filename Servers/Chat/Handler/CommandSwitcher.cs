using Chat.Handler.CRYPT;
using System.Collections.Generic;

namespace Chat
{
    public class CommandSwitcher
    {
        public static void Switch(ChatSession session,Dictionary<string,string> recv)
        {
           
            CRYPTHandler.Handle(session, recv);

        }
    }
}
