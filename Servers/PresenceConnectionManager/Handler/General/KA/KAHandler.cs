using PresenceConnectionManager.Enumerator;

namespace PresenceConnectionManager.Handler.General.KeepAlive
{
    public class KAHandler
    {
        /// <summary>
        /// Polls the connection, and checks for drops
        /// </summary>
        public static void SendKeepAlive(GPCMSession session)
        {
            if (session.PlayerInfo.LoginProcess == LoginStatus.Completed)
            {
                // Try and send a Keep-Alive
                try
                {
                    session.Send(@"\ka\\final\");
                }
                catch
                {
                    session.DisconnectByReason(DisconnectReason.KeepAliveFailed);
                }
            }
        }
    }
}
