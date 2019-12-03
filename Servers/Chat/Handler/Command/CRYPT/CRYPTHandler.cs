using Chat.Structure;
using System.Collections.Generic;

namespace Chat.Handler.Command.CRYPT
{
    public class CRYPTHandler
    {
        public static void Handle(ChatSession session,string[] recv)
        {
            // CRYPT des 1 gamename
            session.chatUserInfo.gameName = recv[3];

            Dictionary<string, object> secretKeyDict = CRYPTQuery.GetSecretKeyFromGame(recv[3]);

            if (secretKeyDict == null)
            {
                session.SendCommand(ChatError.MoreParameters, "CRYPT :Not enough parameters");
                return;
            }

            if (!secretKeyDict.ContainsKey("secretkey"))
            {
                session.SendCommand(ChatError.MoreParameters, "CRYPT :Not enough parameters");
                return;
            }

            string secretKey = secretKeyDict["secretkey"].ToString();

            session.ElevateSecurity(secretKey);
     
        }
    }
}
