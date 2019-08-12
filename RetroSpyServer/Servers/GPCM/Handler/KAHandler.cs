using RetroSpyServer.Servers.GPCM.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.GPCM.Handler
{
    public class KAHandler
    {
        /// <summary>
        /// Polls the connection, and checks for drops
        /// </summary>
        public static void SendKeepAlive(GPCMClient client)
        {
            if (client.PlayerInfo.LoginStatus == LoginStatus.Completed)
            {
                // Try and send a Keep-Alive
                try
                {
                    client.Stream.SendAsync(@"\ka\\final\");
                }
                catch
                {
                    client.Disconnect(DisconnectReason.KeepAliveFailed);
                }
            }
        }
    }
}
