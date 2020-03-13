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
            List<byte> dataList = new List<byte>();
            //first add client public ip and port
            dataList.AddRange(_clientRemoteIP);
            dataList.AddRange(_clientRemotePort);

            //add server keys and keytypes
            dataList.AddRange(_getServerFromQR.GenerateServerKeys(_sbRequest.Keys));
            //add server unique values
            dataList.AddRange(_getServerFromQR.GenerateUniqueValues(_sbRequest.Keys));
            //add server infomation such as public ip etc.
            dataList.AddRange(_getServerFromQR.GenerateServerInfos());
            //after all server information is added we add the end flag
            dataList.AddRange(_AllServersEndFlag);

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

            byte[] plainText = dataList.ToArray();
            session.ToLog($"[Plaintext] {Encoding.ASCII.GetString(plainText)}");

            EnctypeX enx = new EnctypeX();
            _sendingBuffer = enx.EncryptData(secretKey, _sbRequest.Challenge, plainText, 0);

            //save encryption key so we can use in serverinfo request
            session.EncXKey = enx._encxkey;
        }
    }
}

