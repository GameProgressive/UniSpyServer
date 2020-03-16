using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.Encryption;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure.Packet.Request;
using ServerBrowser.Handler.CommandHandler.ServerList.GetServers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ServerBrowser.Handler.CommandHandler.ServerList
{
    public class ServerListHandler : SBHandlerBase
    {
        private byte[] _clientRemoteIP;
        private byte[] _clientRemotePort;
        private ServerListPacket _sbRequest;
        GetServersFromQR _getServerFromQR;
        private readonly byte[] _AllServersEndFlag = new byte[] { 0, 255, 255, 255, 255 };
        public ServerListHandler(SBSession session, byte[] recv) : base(session, recv) { }

        public override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);
            //save client challenge in _sbRequest
            _sbRequest = new ServerListPacket(recv);
            //this is client public ip and port
            IPEndPoint remote = (IPEndPoint)session.Socket.RemoteEndPoint;
            _clientRemoteIP = remote.Address.GetAddressBytes();
            _clientRemotePort = BitConverter.GetBytes((ushort)(remote.Port & 0xFFFF));
        }
        public override void DataOperation(SBSession session, byte[] recv)
        {
            //we can use GetServersFromNetwork class in the future
            _getServerFromQR = new GetServersFromQR(new GetServersFromMemory(), _sbRequest.GameName, _sbRequest.Filter);
        }

        public override void ConstructResponse(SBSession session, byte[] recv)
        {
            List<byte> serversList = new List<byte>();

            //first add client public ip and port
            serversList.AddRange(_clientRemoteIP);
            serversList.AddRange(_clientRemotePort);

            //add server keys and keytypes
            serversList.AddRange(_getServerFromQR.GenerateServerKeys(_sbRequest.Keys));

            //we use NTS string so total unique value list is 0
            serversList.Add(0);

            //add server infomation such as public ip etc.
            serversList.AddRange(_getServerFromQR.GenerateServerInfos(_sbRequest.Keys));

            //after all server information is added we add the end flag
            serversList.AddRange(_AllServersEndFlag);

            //TODO
            // ADDHOC data
            //dataList.AddRange(GetAddHocData());

            //we get secrete key from database
            string secretKey;
            using (var db = new retrospyContext())
            {
                var result = from p in db.Games
                             where p.Gamename == _sbRequest.GameName
                             select new { p.Secretkey };

                if (result.Count() == 1)
                    secretKey = result.First().Secretkey;
                else
                {
                    _errorCode = SBErrorCode.DataOperation;
                    session.ToLog($"Unknown or unsupported game: {_sbRequest.GameName}");
                    return;
                }
            }
            string serverChallenge = "0000000000";


            List<byte> cryptHeader = new List<byte>();
            // we add the message length here
            cryptHeader.Add(2 ^ 0xEC);
            cryptHeader.AddRange(new byte[] { 0, 0 });
            cryptHeader.Add((byte)(serverChallenge.Length ^ 0xEA));
            cryptHeader.AddRange(Encoding.ASCII.GetBytes(serverChallenge));
            //we add game flag here

           
            
            byte[] plainText = serversList.ToArray();


            session.ToLog($"[Plaintext] {Encoding.ASCII.GetString(plainText)}");

            GOAEncryption enc = new GOAEncryption(secretKey, _sbRequest.Challenge, serverChallenge);
            cryptHeader.AddRange(enc.Encrypt(plainText));
           serversList.InsertRange(0, cryptHeader);
            _sendingBuffer = serversList.ToArray();
            session.EncState = enc.State;

        }
    }
}

