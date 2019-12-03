using GameSpyLib.Logging;
using QueryReport.DatabaseQuery;
using QueryReport.Structure;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace QueryReport.Handler.HeartBeat
{
    public class HeartBeatHandler
    {
        /// <summary>
        /// Client or server information come in
        /// </summary>
        /// <param name="server"></param>
        /// <param name="packet"></param>
        public static void HeartbeatResponse(QRServer server, EndPoint endPoint, byte[] buffer)
        {

            byte[] recvKeys = new byte[4];
            //we copy 4 bytes information prepare for reply 
            Array.Copy(buffer, 1, recvKeys, 0, 4);


            byte[] restData = buffer.Skip(5).ToArray();
            string recvData = Encoding.UTF8.GetString(restData);
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
                byte[] sendingBuffer = GenerateChallenge(recvKeys);

                server.SendAsync(endPoint, sendingBuffer);
                string errorMsg = string.Format("[HeartBeat] Invalid Server Data Received From {0}:{1}-{2}", server.Endpoint.Address, server.Endpoint.Port, dataFrag[0]);
                server.ToLog(errorMsg);
                return;
            }
            // We only care about the server data
            string[] serverDataFrag = serverData.Split(new string[] { "\x00" }, StringSplitOptions.None);
            LogWriter.Log.Write(LogLevel.Debug, "[QR] [HeartBeat] Data received From {0}:{1}", server.Endpoint.Address, server.Endpoint.Port);
            LogWriter.Log.Write(LogLevel.Debug, "[QR] [HeartBeat] server info:{0} \t player info:{1} \t team info:{2}", serverData, playerData, teamData);

            GameServerData gameServer = new GameServerData(server.Endpoint);
            // set the country based off ip address if its IPv4

            //we set the server variables
            SetServerVariables(gameServer, serverDataFrag, server.Endpoint);

            LogWriter.Log.Write("[QR] No impliment function for Heartbeatpacket!", LogLevel.Debug);
            //TODO
        }

        private static void SetServerVariables(GameServerData gameServer, string[] serverDataFrag, IPEndPoint remote)
        {
            gameServer.Country = (remote.Address.AddressFamily == AddressFamily.InterNetwork)
               ? GeoIP.GetCountryCode(remote.Address).ToUpperInvariant()
               : "??";

            for (int i = 0; i < serverDataFrag.Length - 1; i += 2)
            {
                //Fetch the properties
                PropertyInfo property = typeof(GameServerData).GetProperty(serverDataFrag[i]);
                if (property == null)
                    continue;

            }
        }

        private static bool IsServerDataValid(string recvData, out string[] dataFrag)
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
        private static byte[] GenerateChallenge(byte[] recvKeys)
        {
            byte[] sendingbuffer = new byte[23];
            sendingbuffer[0] = QR.QRMagic1;
            sendingbuffer[1] = QR.QRMagic2;
            sendingbuffer[2] = QRGameServer.Challenge;
            Array.Copy(recvKeys, 0, sendingbuffer, 3, 4);
            //hard coded challenge, we will make it dynamic later
            byte[] challenge = {0x44, 0x3d, 0x73,0x7e, 0x6a, 0x59, 0x30, 0x30, 0x37, 0x43, 0x39, 0x35, 0x41, 0x42, 0x42, 0x35, 0x37, 0x34,
                                                0x43, 0x43, 0x00};
            Array.Copy(challenge, 0, sendingbuffer, 7, challenge.Length);
            return sendingbuffer;
        }
    }
}
