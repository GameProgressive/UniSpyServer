using GameSpyLib.Common;
using StatsAndTracking.Structure;
using System.Collections.Generic;

namespace Chat.Handler.CRYPT
{
    public class CRYPTHandler
    {
        public static void Handle(ChatSession session,string[] recv)
        {
            // CRYPT des 1 gamename
            session.chatUserInfo.gameName = recv[3];

            //this is a fake response;
/*            string clientKey = GameSpyRandom.GenerateRandomString(16, GameSpyRandom.StringType.Alpha);
            string serverKey = GameSpyRandom.GenerateRandomString(16, GameSpyRandom.StringType.Alpha);*/
            session.ElevateSecurity();
//            session.SendCommand(ChatRPL.SecureKey, "* " + clientKey + " " + serverKey);
        }
    }
}
