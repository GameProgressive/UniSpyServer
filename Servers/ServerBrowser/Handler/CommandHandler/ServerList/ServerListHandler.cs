using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.Encryption;
using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure;
using ServerBrowser.Entity.Structure.Packet.Request;
using ServerBrowser.Entity.Structure.Packet.Response;
using ServerBrowser.Handler.CommandHandler.ServerList.GetServers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ServerBrowser.Handler.CommandHandler.ServerList
{
    public class ServerListHandler : CommandHandlerBase
    {
        private byte[] _clientRemoteIP;
        private byte[] _clientRemotePort;
        private string _serverChallenge;
        private string _secretKey;
        private bool _IsUsingNTSValue = true;
        private ServerListRequest _request;
        private IEnumerable<KeyValuePair<EndPoint, GameServer>> _filteredServers;

        public ServerListHandler(SBSession session, byte[] recv) : base(session, recv) { }

        public override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);
            //save client challenge in _sbRequest
            _request = new ServerListRequest();
            _request.Parse(recv);

            //this is client public ip and port
            IPEndPoint remote = (IPEndPoint)session.Socket.RemoteEndPoint;
            _clientRemoteIP = remote.Address.GetAddressBytes();
            //TODO   //check what is the default port
            _clientRemotePort = BitConverter.GetBytes((ushort)(6500 & 0xFFFF));
        }

        public override void DataOperation(SBSession session, byte[] recv)
        {
            //we can use GetServersFromNetwork class in the future
            _filteredServers = GetServersFromQR.GetFilteredServer(new GetServersFromMemory(), _request.GameName, _request.Filter);

            if (_filteredServers.Count() == 0 && _request.UpdateOption != SBServerListUpdateOption.NoServerList)
            {
                _errorCode = SBErrorCode.NoServersFound;
            }
        }

        public override void ConstructResponse(SBSession session, byte[] recv)
        {
            List<byte> serverList = new List<byte>();

            //first add client public ip and port
            serverList.AddRange(_clientRemoteIP);
            serverList.AddRange(_clientRemotePort);
            //add server keys and keytypes
            serverList.AddRange(GenerateServerKeys());
            //we use NTS string so total unique value list is 0
            serverList.AddRange(GenerateUniqueValue());
            //add server infomation such as public ip etc.
            serverList.AddRange(GenerateServersInfo());
            //after all server information is added we add the end flag
            serverList.AddRange(SBStringFlag.AllServerEndFlag);

            //we get secrete key from database
            if (!GetSecretKey())
            {
                session.ToLog($"Unknown or unsupported game: {_request.GameName}");
                return;
            }

            _serverChallenge = "0000000000";

            session.ToLog($"[Plaintext] {Encoding.ASCII.GetString(serverList.ToArray())}");
            GOAEncryption enc;
            if (session.EncState == null)
            {
                enc = new GOAEncryption(_secretKey, _request.Challenge, _serverChallenge);
            }
            else
            {
                enc = new GOAEncryption(session.EncState);
            }
           
            _sendingBuffer = new ServerListResponse().
                CombineHeaderAndContext
                (
                    enc.Encrypt(serverList.ToArray()),
                    _serverChallenge
                );
            //refresh encryption state
            session.EncState = enc.State;
        }


        private List<byte> GenerateServerKeys()
        {
            List<byte> data = new List<byte>();

            switch (_request.UpdateOption)
            {
                case SBServerListUpdateOption.NoServerList:
                    //we do not need to add keys
                    data.Add(0);
                    break;
                case SBServerListUpdateOption.SendRequestedField:
                    data.Add((byte)_request.FieldList.Length);
                    foreach (var key in _request.FieldList)
                    {
                        data.Add((byte)SBKeyType.String);
                        data.AddRange(Encoding.ASCII.GetBytes(key));
                        data.Add(0);
                    }
                    break;
            }
            return data;
        }
        private List<byte> GenerateUniqueValue()
        {

            List<byte> data = new List<byte>();
            switch (_request.UpdateOption)
            {
                case SBServerListUpdateOption.NoServerList:
                    break;
                case SBServerListUpdateOption.SendRequestedField:
                    if (_IsUsingNTSValue)
                    {
                        //we do not add unique value here
                        //because we are using NTS string
                        data.Add(0);
                    }
                    else
                    {
                        //we add unique value here
                    }
                    break;
            }

            return data;
        }
        private List<byte> GenerateServersInfo()
        {
            List<byte> data = new List<byte>();

            foreach (var server in _filteredServers)
            {
                data.AddRange(GenerateServerInfoHeader(server));

                switch (_request.UpdateOption)
                {
                    case SBServerListUpdateOption.SendRequestedField:
                        //add every value to list
                        if (_IsUsingNTSValue)
                        {
                            foreach (var key in _request.FieldList)
                            {
                                data.Add(SBStringFlag.NTSStringFlag);
                                data.AddRange(Encoding.ASCII.GetBytes(server.Value.ServerData.StandardKeyValue[key]));
                                data.Add(SBStringFlag.StringSpliter);
                            }
                        }
                        else
                        {
                            //do unique values method
                        }
                        break;
                    case SBServerListUpdateOption.NoServerList:
                        break;
                }
            }
            return data;
        }

        private List<byte> GenerateServerInfoHeader(KeyValuePair<EndPoint, GameServer> server)
        {
            // you will only have HasKeysFlag or HasFullRule you can not have both
            List<byte> header = new List<byte>();

            //add has key flag
            header.Add((byte)GameServerFlags.HasKeysFlag);

            //we add server public ip here
            header.AddRange(BitConverter.GetBytes(server.Value.PublicIP));

            //we check host port is standard port or not
            CheckNonStandardPort(header, server);

            // now we check if there are private ip
            CheckPrivateIP(header, server);

            // we check private port here
            CheckPrivatePort(header, server);

            //TODO we have to check icmp_ip_flag

            return header;
        }
        private void CheckPrivateIP(List<byte> header, KeyValuePair<EndPoint, GameServer> server)
        {
            // now we check if there are private ip
            if (server.Value.ServerData.StandardKeyValue.ContainsKey("localip0"))
            {
                header[0] ^= (byte)GameServerFlags.PrivateIPFlag;
                byte[] address = IPAddress.Parse(server.Value.ServerData.StandardKeyValue["localip0"]).GetAddressBytes();
                header.AddRange(address);
            }
            else if (server.Value.ServerData.StandardKeyValue.ContainsKey("localip1"))
            {
                header[0] ^= (byte)GameServerFlags.PrivateIPFlag;
                byte[] address = IPAddress.Parse(server.Value.ServerData.StandardKeyValue["localip1"]).GetAddressBytes();
                header.AddRange(address);
            }
        }
        private void CheckNonStandardPort(List<byte> header, KeyValuePair<EndPoint, GameServer> server)
        {
            //we check host port is standard port or not
            if (server.Value.ServerData.StandardKeyValue.ContainsKey("hostport"))
            {
                if (server.Value.ServerData.StandardKeyValue["hostport"] != "6500")
                {
                    header[0] ^= (byte)GameServerFlags.NonStandardPort;
                    //we do not need to reverse port bytes
                    byte[] port = BitConverter.GetBytes(ushort.Parse(server.Value.ServerData.StandardKeyValue["hostport"]));

                    header.AddRange(port);
                }
            }
        }
        private void CheckPrivatePort(List<byte> header, KeyValuePair<EndPoint, GameServer> server)
        {
            // we check private port here
            if (server.Value.ServerData.StandardKeyValue.ContainsKey("localport"))
            {
                header[0] ^= (byte)GameServerFlags.NonStandardPrivatePortFlag;
                byte[] localPort =
                 BitConverter.GetBytes(ushort.Parse(server.Value.ServerData.StandardKeyValue["localport"]));

                header.AddRange(localPort);
            }
        }

        private bool GetSecretKey()
        {
            using (var db = new retrospyContext())
            {
                var result = from p in db.Games
                             where p.Gamename == _request.GameName
                             select new { p.Secretkey };

                if (result.Count() == 1)
                {
                    _secretKey = result.First().Secretkey;
                    return true;
                }
                else
                {
                    _errorCode = SBErrorCode.DataOperation;
                    return false;
                }
            }
        }
    }
}
