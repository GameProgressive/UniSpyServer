using GameSpyLib.Logging;
using GameSpyLib.Network;
using QueryReport.GameServerInfo;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace QueryReport.Handler
{
    public class HeartBeatHandler
    {
        /// <summary>
        /// Client or server information come in
        /// </summary>
        /// <param name="server"></param>
        /// <param name="packet"></param>
        public static void HeartbeatResponse(QRServer server, UdpPacket packet)
        {
            IPEndPoint remote = (IPEndPoint)packet.AsyncEventArgs.RemoteEndPoint;
            string recvData = Encoding.UTF8.GetString(packet.BytesRecieved);
            string[] dataFrag;
            string serverData, playerData, teamData;
            if (IsServerDataValid(recvData, out dataFrag))
            {
                serverData = dataFrag[0];
                playerData = dataFrag[1];
                teamData = dataFrag[2];
            }
            else
            {
                //we revieved a wrong data, we have to send challege to game server

                LogWriter.Log.Write(LogLevel.Debug, "[QR] [HeartBeat] Invalid Server Data Received From {0}:{1}-{2}", remote.Address, remote.Port, dataFrag[0]);
                return;
            }
            // We only care about the server data
            string[] serverDataFrag = serverData.Split(new string[] { "\x00" }, StringSplitOptions.None);
            LogWriter.Log.Write(LogLevel.Debug, "[QR] [HeartBeat] Data received From {0}:{1}", remote.Address, remote.Port);
            LogWriter.Log.Write(LogLevel.Debug, "[QR] [HeartBeat] server info:{0} \t player info:{1} \t team info:{2}", serverData, playerData, teamData);

            GameServer gameServer = new GameServer(remote);
            // set the country based off ip address if its IPv4
           
            //we set the server variables
            SetServerVariables(gameServer, serverDataFrag, remote);




            LogWriter.Log.Write("[QR] No impliment function for Heartbeatpacket!", LogLevel.Debug);
            //TODO
        }

        private static void SetServerVariables(GameServer gameServer, string[] serverDataFrag,IPEndPoint remote)
        {
            gameServer.Country = (remote.Address.AddressFamily == AddressFamily.InterNetwork)
               ? GeoIP.GetCountryCode(remote.Address).ToUpperInvariant()
               : "??";

            for (int i = 0; i < serverDataFrag.Length - 1; i += 2)
            {
                //Fetch the properties
                PropertyInfo property = typeof(GameServer).GetProperty(serverDataFrag[i]);
                if (property == null)
                    continue;

            }
        }

        public static bool IsServerDataValid(string recvData, out string[] dataFrag)
        {
            dataFrag = recvData.Split(new string[] { "\x00\x00\x00", "\x00\x00\x02" }, StringSplitOptions.None);
            if (dataFrag.Length != 3 && !recvData.EndsWith("\x00\x00"))
            {
                // server data is not valid so we ignore
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
