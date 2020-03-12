using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.Encryption;
using QueryReport.Entity.Structure;
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
        private byte[] _remoteIP;
        private byte[] _remotePort;
        private ServerListPacket _sbRequest;
        private IEnumerable<KeyValuePair<EndPoint, GameServer>> _onlineServers;
        int _totalValueNumber;
        int _totalKeysNumber;
        GetServerFromQR _getServerFromQR;
        public ServerListHandler(SBSession session, byte[] recv) : base(session, recv)
        {
        }

        public override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);
            //save client challenge in _sbRequest
            _sbRequest = new ServerListPacket(recv);

            //this is client public ip and port
            IPEndPoint remote = (IPEndPoint)session.Socket.RemoteEndPoint;
            _remoteIP = remote.Address.GetAddressBytes();

            //TODO
            //we have to make sure the port number should be reverse or not
            _remotePort = BitConverter.GetBytes((ushort)(remote.Port & 0xFFFF));
        }
        public override void DataOperation(SBSession session, byte[] recv)
        {
            //_onlineServers = QueryReport.Server.QRServer.GameServerList.
            //  Where(c => c.Value.ServerKeyValue.Data["gamename"]
            //  == _sbRequest.GameName);
            _getServerFromQR = new GetServerFromQR(new GetServersFromMemory(), _sbRequest.GameName, _sbRequest.Filter);
        }

        public override void ConstructResponse(SBSession session, byte[] recv)
        {
            List<byte> dataList = new List<byte>();
            dataList.AddRange(_remoteIP);
            dataList.AddRange(_remotePort);

            //dataList.AddRange(GetServersKeys(_onlineServers));
            //dataList.AddRange(GetUniqueValues(_onlineServers));
            //dataList.AddRange(GetServerInfo(_onlineServers));
            dataList.AddRange(_getServerFromQR.GenerateServerKeys(_sbRequest.Keys));
            dataList.AddRange(_getServerFromQR.GenerateUniqueValues(_sbRequest.Keys));
            dataList.AddRange(_getServerFromQR.GenerateServerInfos());

            //TODO
            // ADDHOC data
            //dataList.AddRange(GetAddHocData());

            //we get secrete key from database
            string secretKey = FindGameSecreteKey();

            if (secretKey == null)
            {
                session.ToLog($"Unknown or unsupported game: {_sbRequest.GameName}");
                _errorCode = SBErrorCode.DataOperation;
                return;
            }

            byte[] buffer = dataList.ToArray();
            session.ToLog($"[Plaintext] {Encoding.ASCII.GetString(buffer)}");

            EnctypeX enx = new EnctypeX();
            _sendingBuffer = enx.EncryptData(secretKey, _sbRequest.Challenge, buffer, 0);

            //save encryption key so we can use in serverinfo request
            session.EncXKey = enx._encxkey;
        }

        private string FindGameSecreteKey()
        {
            using (var db = new retrospyContext())
            {
                var secretKeyResult = from p in db.Games
                                      where p.Gamename == _sbRequest.GameName
                                      select new { p.Secretkey };

                if (secretKeyResult.Count() < 1)
                    return null;

                return secretKeyResult.First().Secretkey;
            }
        }
    }
}

