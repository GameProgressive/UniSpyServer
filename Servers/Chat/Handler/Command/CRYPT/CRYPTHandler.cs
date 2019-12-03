using GameSpyLib.Common;
using System.Collections.Generic;

namespace Chat.Handler.CRYPT
{
    public class CRYPTHandler
    {
        public static void Handle(ChatSession session,string[] recv)
        {
            // CRYPT des 1 gamename
            session.chatUserInfo.gameName = recv[3];
            session.ElevateSecurity();
//            
        }
    }
}
