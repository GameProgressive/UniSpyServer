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
    public class ServerListHandler : SBHandlerBase
    {
        byte[] _remoteIP;
        byte[] _remotePort;
        SBRequestPacket _sbRequest;
        public ServerListHandler(SBSession session, byte[] recv) : base(session, recv)
        {
        }

        public override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);
            _sbRequest = new SBRequestPacket(recv);

            _remoteIP = ((IPEndPoint)session.Socket.RemoteEndPoint).Address.GetAddressBytes();
            //TODO we have to make sure the port number
            _remotePort = BitConverter.GetBytes((ushort)(((IPEndPoint)session.Socket.RemoteEndPoint).Port & 0xFFFF));

        }

        public override void ConstructResponse(SBSession session, byte[] recv)
        {
            List<byte> data = new List<byte>();
            data.AddRange(_remoteIP);
            data.AddRange(_remotePort);
            //the key lenth, because we manually added ping so we add one here
            data.Add((byte)(_sbRequest.DataField.Length + 1));

            //The following byte should be keyType: maybe serverkey playerkey teamkey

            foreach (var field in _sbRequest.DataField)
            {
                data.Add((byte)SBKeyType.String);
                data.AddRange(Encoding.ASCII.GetBytes(field));
                data.Add(0);
            }
            //manually add ping
            data.Add((byte)SBKeyType.Byte);
            data.AddRange(Encoding.ASCII.GetBytes("ping"));
            data.Add(0);
            string gamename = Encoding.ASCII.GetString(_sbRequest.QueryFromGameName);
            var onlineServers = QueryReport.Server.QRServer.GameServerList.
                Where(c => c.Value.ServerKeyValue.Data["gamename"] == gamename);

            //we add the number of values, because we manually add ping so here should be add one
            int valueCount = onlineServers.Count() * _sbRequest.DataField.Length + 1;
            data.Add(Convert.ToByte(valueCount));

            foreach (var server in onlineServers)
            {
                for (int i = 0; i < _sbRequest.DataField.Length; i++)
                {
                    string temp = server.Value.ServerKeyValue.Data[_sbRequest.DataField[i]];
                    data.AddRange(Encoding.ASCII.GetBytes(temp));
                    data.Add(0); // Field Seperator
                }
                //this is ping value
                data.Add(Convert.ToByte(32));
                data.Add(0);
            }
            byte serverFlag = 0;
            data.Add(serverFlag);
            data.AddRange(new byte[] { 255, 255, 255, 255 });

            // we need some kind of information about server (ad hoc information)
            List<byte> tempList = new List<byte>();
            tempList.Add((byte)SBAdHocType.PushKeysMessage);
            tempList.Add(1);
            tempList.Add((byte)SBKeyType.String);
            tempList.AddRange(Encoding.ASCII.GetBytes("retrospy"));
            tempList.Add(0);

            byte[] buffer = data.ToArray();
            Console.WriteLine(Encoding.ASCII.GetString(buffer));
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
            _sendingBuffer = new EnctypeX()
                .EncryptData
                (
                    Encoding.ASCII.GetBytes(secretkey),
                    _sbRequest.Challenge,
                    buffer, 0
                ).
                ToArray();
        }
    }
}

