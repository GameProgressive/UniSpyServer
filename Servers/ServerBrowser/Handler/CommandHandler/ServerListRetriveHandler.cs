using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ServerBrowser.Handler.CommandHandler
{
    public class ServerListRetriveHandler
    {
        public ServerListRetriveHandler(SBSession session, string recv)
        {
            Handle(session, recv);
        }

        public void Handle(SBSession session, string request)
        {

            string header, dataPartition;

            string[] parts = request.Split(new string[] { "\x00\x00" }, StringSplitOptions.RemoveEmptyEntries);
            //we have to check the request Header
            string requestHeader = request.Substring(0, 9);

            string[] gameInfo = request.Substring(9).Split(new string[] { "\x00\\" }, StringSplitOptions.RemoveEmptyEntries);

            string[] restData = gameInfo[0].Split('\x00');
            if (restData[0] == restData[1])
            { 
            
            }
            string gameName = restData[0];
    
            string filter = restData[2];
            string[] dataFields = gameInfo[1].Split('\\', StringSplitOptions.RemoveEmptyEntries);


            byte[] remoteIP = ((IPEndPoint)session.Remote).Address.GetAddressBytes();
            //TODO we have to make sure the port number
            byte[] remotePort = BitConverter.GetBytes(((IPEndPoint)session.Remote).Port);


            List<byte> data = new List<byte>();
            data.AddRange(remoteIP);
            data.AddRange(remotePort);
            data.Add((byte)dataFields.Length);
            data.Add(0);
            foreach (var field in dataFields)
            {
                data.AddRange(Encoding.ASCII.GetBytes(field));
                data.AddRange(new byte[] { 0, 0 });
            }

            var onlineServers = QueryReport.Server.QRServer.GameServerList.Select(x => x.Value).Where(c => c.ServerInfo.Data["gamename"]== gameName);
            foreach (var server in onlineServers)
            {
                byte[] portBytes = server.RemotePort;
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(portBytes, 0, portBytes.Length);
                data.Add(81);
                data.AddRange(server.RemoteIP);
                //check port
                data.AddRange(remotePort);
                data.Add(255);

                //for (int i = 0; i < dataFields.Length; i++)
                //{
                //    data.AddRange(Encoding.UTF8.GetBytes(GetField(server, dataFields[i])));
                //    if (i < dataFields.Length - 1)
                //        data.AddRange(new byte[] { 0, 255 }); // Field Seperator
                //}

            }
        }
    }
}

