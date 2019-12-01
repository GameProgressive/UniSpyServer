using GameSpyLib.Common;
using StatsAndTracking.Structure;
using System.Collections.Generic;

namespace Chat.Handler.CRYPT
{
    public class CRYPTHandler
    {
        public static void Handle(ChatSession session,string[] recv)
        {
            // first check the is the game recv["gamename"] avaliable

            //this is a fake response;
            string clientKey = GameSpyRandom.GenerateRandomString(16, GameSpyRandom.StringType.Alpha);
            string serverKey = GameSpyRandom.GenerateRandomString(16, GameSpyRandom.StringType.Alpha);
            string sendingBuffer = ":s " + ChatRPL.SecureKey + " * " + clientKey + " " + serverKey + "\r\n";
            session.SendAsync(sendingBuffer);

        }
    }
}
