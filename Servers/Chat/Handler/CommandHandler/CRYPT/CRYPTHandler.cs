using Chat.Entity.Structure;

namespace Chat.Handler.CommandHandler.CRYPT
{
    public class CRYPTHandler
    {
        public static void Handle(ChatSession session, string[] recv)
        {
            // CRYPT des 1 gamename
            session.chatUserInfo.gameName = recv[3];

            string secretKey = CRYPTQuery.GetSecretKeyFromGame(recv[3]);

            if (secretKey == null)
            {
                session.SendCommand(ChatError.MoreParameters, "CRYPT :Secret key not found!");
                return;
            }

            session.ElevateSecurity(secretKey);
        }
    }
}
