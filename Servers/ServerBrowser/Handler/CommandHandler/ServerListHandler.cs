using GameSpyLib.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ServerBrowser.Handler.CommandHandler
{
    public class ServerListHandler
    {

        public ServerListHandler(SBSession session, string recv)
        {
            Handle(session, recv);
        }

        public void Handle(SBSession session, string request)
        {

            string[] parts = request.Split(new string[] { "\x00\x00" }, StringSplitOptions.RemoveEmptyEntries);
            //we have to check the request Header
            string requestHeader = request.Substring(0, 9);
            string[] gameInfo = request.Substring(9).Split(new string[] { "\x00\\" }, StringSplitOptions.RemoveEmptyEntries);
            string[] restData = gameInfo[0].Split('\x00');

            //we do not care about the restData[0] it is used for game development test
            //string testGameName = restData[0];
            string gameName = restData[1];
            string filter;
            if (restData.Length == 3)
            { filter= restData[2]; }
            
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

            var onlineServers = QueryReport.Server.QRServer.GameServerList.
                Where(c => c.Value.ServerInfo.Data["gamename"]== gameName);
            foreach (var server in onlineServers)
            {
                byte[] portBytes = Encoding.ASCII.GetBytes(server.Value.ServerInfo.Data["hostport"]);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(portBytes, 0, portBytes.Length);
                //maybe is ping
                data.Add(81);
                data.AddRange(server.Value.RemoteIP);
                //check port
                data.AddRange(server.Value.RemotePort);
                data.Add(255);

                for (int i = 0; i < dataFields.Length; i++)
                {
                    data.AddRange(Encoding.UTF8.GetBytes("1"));
                    if (i < dataFields.Length - 1)
                        data.AddRange(new byte[] { 0, 255 }); // Field Seperator
                }
                data.Add(0);
            }
            data.AddRange(new byte[] { 0, 255, 255, 255, 255 });
            byte[] sendingbuffer = data.ToArray();
            string secretkey = "HA6zkS";
            byte[] encBuffer = 
                Enctypex.Encode(Encoding.ASCII.GetBytes(secretkey), 
                Encoding.ASCII.GetBytes(gameName), sendingbuffer, 
                sendingbuffer.Length);
            session.SendAsync(encBuffer);
        }
    }
}

