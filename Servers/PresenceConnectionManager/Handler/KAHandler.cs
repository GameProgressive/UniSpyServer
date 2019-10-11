using PresenceConnectionManager.Enumerator;

namespace PresenceConnectionManager.Handler
{
    public class KAHandler
    {
        /// <summary>
        /// Polls the connection, and checks for drops
        /// </summary>
        public static void SendKeepAlive(GPCMSession session)
        {
            if (session.LoginProcess == LoginStatus.Completed)
            {
                // Try and send a Keep-Alive
                try
                {
                    session.SendAsync(@"\ka\\final\");
                }
                catch
                {
                    session.DisconnectByReason(DisconnectReason.KeepAliveFailed);
                }
            }
        }
    }
}
