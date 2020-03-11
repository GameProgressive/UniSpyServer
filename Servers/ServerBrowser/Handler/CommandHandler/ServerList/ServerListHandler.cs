using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.Encryption;
using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure.Packet.Request;
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
        public ServerListHandler(SBSession session, byte[] recv) : base(session, recv)
        {
        }

        public override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);
            _sbRequest = new ServerListPacket(recv);
            //save challenge
            IPEndPoint remote = (IPEndPoint)session.Socket.RemoteEndPoint;
            _remoteIP = remote.Address.GetAddressBytes();
            //TODO we have to make sure the port number
            _remotePort = BitConverter.GetBytes((ushort)(remote.Port & 0xFFFF));

        }
        public override void DataOperation(SBSession session, byte[] recv)
        {
            _onlineServers = QueryReport.Server.QRServer.GameServerList.
              Where(c => c.Value.ServerKeyValue.Data["gamename"]
              == _sbRequest.GameName);
        }

        public override void ConstructResponse(SBSession session, byte[] recv)
        {
            List<byte> dataList = new List<byte>();
            dataList.AddRange(_remoteIP);
            dataList.AddRange(_remotePort);

            dataList.AddRange(GetServersKeys(_onlineServers));
            dataList.AddRange(GetUniqueValues(_onlineServers));
            dataList.AddRange(GetServerInfo(_onlineServers));


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


        private byte[] GetServersKeys(IEnumerable<KeyValuePair<EndPoint, GameServer>> onlineServers)
        {
            List<byte> data = new List<byte>();

            //the key lenth, because we manually added ping so we add one here
            _totalKeysNumber = _sbRequest.Keys.Length;
            data.Add((byte)_totalKeysNumber);

            //The following byte should be keyType: maybe serverkey playerkey teamkey

            foreach (var field in _sbRequest.Keys)
            {
                //get every keys key type
                data.Add((byte)SBKeyType.String);
                data.AddRange(Encoding.ASCII.GetBytes(field));
                data.Add(0);
            }
            ////manually add key ping
            //data.Add((byte)SBKeyType.Byte);
            //data.AddRange(Encoding.ASCII.GetBytes("ping"));
            //data.Add(0);
            return data.ToArray();
        }

        private byte[] GetUniqueValues(IEnumerable<KeyValuePair<EndPoint, GameServer>> onlineServers)
        {
            List<byte> data = new List<byte>();
            //this is total value's number include ping value
            _totalValueNumber = _totalKeysNumber * onlineServers.Count();
            data.Add(Convert.ToByte(_totalValueNumber));

            foreach (var server in onlineServers)
            {
                foreach (var field in _sbRequest.Keys)
                {
                    string temp = server.Value.ServerKeyValue.Data[field];
                    //data.Add(Convert.ToByte(temp.Length));
                    data.AddRange(Encoding.ASCII.GetBytes(temp));
                    data.Add(0); // Field Seperator
                }
                //this is ping value
                //TODO implement real ping system
                //byte pingValue = Convert.ToByte(server.Value.ServerKeyValue.Data["ping"]);
                //data.Add(pingValue);
                //data.Add(0);
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
                                      where p.Gamename == _sbRequest.GameName
                                      select new { p.Secretkey };

                if (secretKeyResult.Count() < 1)
                    return null;

                return secretKeyResult.First().Secretkey;
            }
        }

    }
}

