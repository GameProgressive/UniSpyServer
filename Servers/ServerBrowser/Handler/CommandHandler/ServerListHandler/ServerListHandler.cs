using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.Encryption;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ServerBrowser.Handler.CommandHandler.ServerListHandler
{
    public class ServerListHandler
    {
        public ServerListHandler(SBSession session, byte[] recv)
        {
            Handle(session, recv);
        }

        public void Handle(SBSession session, byte[] recv)
        {
            QR2Query query = new QR2Query(recv);

            byte[] remoteIP = ((IPEndPoint)session.Socket.RemoteEndPoint).Address.GetAddressBytes();
            //TODO we have to make sure the port number
            byte[] remotePort = BitConverter.GetBytes((ushort)(((IPEndPoint)session.Socket.RemoteEndPoint).Port & 0xFFFF));


            List<byte> data = new List<byte>();
            data.AddRange(remoteIP);
            data.AddRange(remotePort);
            //the key lenth
            data.Add((byte)query.DataField.Length);

            //The following byte should be keyType: maybe serverkey playerkey teamkey

            foreach (var field in query.DataField)
            {
                data.Add((byte)SBKeyType.String);
                data.AddRange(Encoding.ASCII.GetBytes(field));
                data.Add(0);
            }

            string gamename = Encoding.ASCII.GetString(query.QueryFromGameName);
            var onlineServers = QueryReport.Server.QRServer.GameServerList.
                Where(c => c.Value.ServerInfo.Data["gamename"] == gamename);

            //we add the number of values
            data.Add(Convert.ToByte(onlineServers.Count() * query.DataField.Length));

            foreach (var server in onlineServers)
            {
                for (int i = 0; i < query.DataField.Length; i++)
                {
                    string temp = server.Value.ServerInfo.Data[query.DataField[i]];
                    data.AddRange(Encoding.ASCII.GetBytes(temp));
                    data.Add(0); // Field Seperator
                }
            }

            data.AddRange(new byte[] { 0, 255, 255, 255, 255 });
            // we need some kind of information about server


            byte[] sendingbuffer = data.ToArray();
            Console.WriteLine(Encoding.ASCII.GetString(sendingbuffer));
            string secretkey = "HA6zkS";
            //using (var db = new RetrospyDB())
            //{
            //    var secretKey = from p in db.Games
            //                    where p.Gamename == Encoding.ASCII.GetString(query.QueryFromGameName)
            //                    select new { p.Secretkey };
            //    if (secretKey == null)
            //    {
            //        return;
            //    }
            //}
            byte[] _sendingBuffer = new EnctypeX()
                .EncryptData
                (
                    Encoding.ASCII.GetBytes(secretkey),
                    query.Challenge,
                    sendingbuffer, 0
                ).
                ToArray();
            session.SendAsync(_sendingBuffer);
        }
        private SBKeyType GetSBKeyType(byte[] data)
        {
            if (data.Length == 1)
            {
                return SBKeyType.Byte;
            }
            else if (data.Length == 2)
            {
                return SBKeyType.Short;
            }
            else
            {
                return SBKeyType.String;
            }
        }
    }
}

