using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.Encryption;
using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure;
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
        private IEnumerable<KeyValuePair<EndPoint, GameServer>> onlineServers;

        public ServerListHandler(SBSession session, byte[] recv) : base(session, recv)
        {
        }

        public override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);
            _sbRequest = new ServerListPacket(recv);

            _remoteIP = ((IPEndPoint)session.Socket.RemoteEndPoint).Address.GetAddressBytes();
            //TODO we have to make sure the port number
            _remotePort = BitConverter.GetBytes((ushort)(((IPEndPoint)session.Socket.RemoteEndPoint).Port & 0xFFFF));

        }
        public override void DataOperation(SBSession session, byte[] recv)
        {
            onlineServers = QueryReport.Server.QRServer.GameServerList.
              Where(c => c.Value.ServerKeyValue.Data["gamename"]
              == Encoding.ASCII.GetString(_sbRequest.QueryFromGameName));
        }

        public override void ConstructResponse(SBSession session, byte[] recv)
        {
            List<byte> dataList = new List<byte>();
            dataList.AddRange(_remoteIP);
            dataList.AddRange(_remotePort);

            dataList.AddRange(GetServersKeys(onlineServers));
            dataList.AddRange(GetUniqueValues(onlineServers));
            dataList.AddRange(GetServerInfo(onlineServers));

            //dataList.AddRange(GetServersInfo());
            byte[] preSendingBuffer = dataList.ToArray();

            //we get secrete key from database
            string gameSk = FindGameSecreteKey();

            if (gameSk == null)
            {
                session.ToLog($"Unknown or unsupported game: {_sbRequest.QueryFromGameName}");
                _errorCode = SBErrorCode.DataOperation;
                return;
            }

            byte[] secretKey = Encoding.ASCII.GetBytes(gameSk);

            EnctypeX enx = new EnctypeX();

            _sendingBuffer = enx.EncryptData(secretKey,
                    _sbRequest.Challenge,
                    preSendingBuffer, 0);
        }
        int _totalKeysNumber;
      
        private byte[] GetServersKeys(IEnumerable<KeyValuePair<EndPoint, GameServer>> onlineServers)
        {
            List<byte> data = new List<byte>();

            //the key lenth, because we manually added ping so we add one here
            _totalKeysNumber = _sbRequest.DataField.Length + 1;
            data.Add((byte)_totalKeysNumber);

            //The following byte should be keyType: maybe serverkey playerkey teamkey

            foreach (var field in _sbRequest.DataField)
            {
                //get every keys key type
                data.Add((byte)SBKeyType.String);
                data.AddRange(Encoding.ASCII.GetBytes(field));
                data.Add(0);
            }
            //manually add key ping
            data.Add((byte)SBKeyType.Byte);
            data.AddRange(Encoding.ASCII.GetBytes("ping"));
            data.Add(0);

            return data.ToArray();
        }
        int _totalValueNumber;
        private byte[] GetUniqueValues(IEnumerable<KeyValuePair<EndPoint, GameServer>> onlineServers)
        {
            List<byte> data = new List<byte>();
            //this is total value's number include ping value
            _totalValueNumber = _totalKeysNumber * onlineServers.Count();
            data.Add(Convert.ToByte( _totalValueNumber));

            foreach (var server in onlineServers)
            {
                foreach (var field in _sbRequest.DataField)
                {
                    string temp = server.Value.ServerKeyValue.Data[field];
                    //data.Add(Convert.ToByte(temp.Length));
                    data.AddRange(Encoding.ASCII.GetBytes(temp));
                    data.Add(0); // Field Seperator
                }
                //this is ping value
                //TODO implement real ping system
                byte pingValue = Convert.ToByte(server.Value.ServerKeyValue.Data["ping"]);
                data.Add(pingValue);
                data.Add(0);
            }
            return data.ToArray();
        }

        private byte[] GetServerInfo(IEnumerable<KeyValuePair<EndPoint, GameServer>> onlineServers)
        {
            List<byte> data = new List<byte>();

            foreach (var server in onlineServers)
            {
                data.Add((byte)GameServerFlags.HasKeysFlag); // Server flags !
                data.AddRange(server.Value.RemoteIP);

                //We have to numberd string by its sequence
                for (int uniqueValueIndex = 0; uniqueValueIndex < _totalKeysNumber; uniqueValueIndex++)
                {
                    data.Add((byte)uniqueValueIndex);
                }
            }

            data.Add((byte)GameServerFlags.ServerInfoEndFlag);
            data.AddRange(new byte[] { 255, 255, 255, 255 });

            return data.ToArray();
        }

        private string FindGameSecreteKey()
        {
            using (var db = new retrospyContext())
            {
                var secretKeyResult = from p in db.Games
                                      where p.Gamename == Encoding.ASCII.GetString(_sbRequest.QueryFromGameName)
                                      select new { p.Secretkey };

                if (secretKeyResult.Count() < 1)
                    return null;

                return secretKeyResult.First().Secretkey;
            }
        }

    }
}

