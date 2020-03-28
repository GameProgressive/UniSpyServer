using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.Encryption;
using GameSpyLib.Extensions;
using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure;
using ServerBrowser.Entity.Structure.Packet.Request;
using ServerBrowser.Entity.Structure.Packet.Response;

namespace ServerBrowser.Handler.CommandHandler.ServerList
{
    public class ServerListHandler : CommandHandlerBase
    {
        private byte[] _clientRemoteIP;
        private byte[] _gameServerDefaultHostPort;
        private string _secretKey;
        private ServerListRequest _request = new ServerListRequest();

        private List<DedicatedGameServer> _redisServer;
        private List<Grouplist> _redisGroup;

        public ServerListHandler(SBSession session, byte[] recv) : base(session, recv) { }

        public override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);
            //save client challenge in _request
            _request.Parse(recv);

            //this is client public ip and default query port
            _clientRemoteIP = ((IPEndPoint)session.Socket.RemoteEndPoint).Address.GetAddressBytes();
            //TODO   //check what is the default port
            _gameServerDefaultHostPort = BitConverter.GetBytes((ushort)(6500 & 0xFFFF));
        }

        public override void DataOperation(SBSession session, byte[] recv)
        {
            switch (_request.UpdateOption)
            {
                case SBServerListUpdateOption.GeneralRequest:
                   //_filteredServers =
                  //      GetServersFromQR.GetFilteredServer(new GetServersFromMemory(), _request.GameName, _request.Filter);
                    _redisServer =
                        RetroSpyRedisExtensions.GetDedicatedGameServers<DedicatedGameServer>(_request.GameName);
                    break;
                case SBServerListUpdateOption.NoServerList:
                    //we do not need to retrive server for this
                    break;
                case SBServerListUpdateOption.PushUpdates:
                    break;
                case SBServerListUpdateOption.SendGroups:
                    //we need to search group in database
                    //_filteredGroups =
                    //    GetGroupsFromQR.GetFilteredGroups(new GetGroupsFromDatabase(), _request.GameName, _request.Filter);
                    break;
            }
            //we can use GetServersFromNetwork class in the future


            if (_redisServer.Count() == 0)
            {
                _errorCode = SBErrorCode.NoServersFound;
            }
        }

        public override void ConstructResponse(SBSession session, byte[] recv)
        {
            List<byte> serverList = new List<byte>();

            //first add client public ip and port
            serverList.AddRange(_clientRemoteIP);
            serverList.AddRange(_gameServerDefaultHostPort);
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
            session.LogPlainText(Encoding.ASCII.GetString(serverList.ToArray()));

            GOAEncryption enc =
                new GOAEncryption(_secretKey, _request.Challenge, SBServer.ServerChallenge);

            _sendingBuffer = new ServerListResponse().
                CombineHeaderAndContext
                (
                    enc.Encrypt(serverList.ToArray()),
                     SBServer.ServerChallenge
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
                    //we do not need to add anything here client will only read game port
                    data.Add(0);
                    break;
                case SBServerListUpdateOption.GeneralRequest:
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
                    //we do not need to add anything here client will only read game port
                    break;
                case SBServerListUpdateOption.GeneralRequest:
                    //we do not add unique value here
                    //because we are using NTS string
                    data.Add(0);
                    break;
                case SBServerListUpdateOption.PushUpdates:
                    break;
                case SBServerListUpdateOption.SendGroups:
                    // we use NTS string to parse values so we do not add things here
                    data.Add(0);
                    break;
            }

            return data;
        }
        private List<byte> GenerateServersInfo()
        {
            List<byte> data = new List<byte>();

            foreach (var server in _redisServer)
            {
                data.AddRange(GenerateServerInfoHeader(server));

                switch (_request.UpdateOption)
                {
                    case SBServerListUpdateOption.GeneralRequest:
                        //add every value to list
                        foreach (var key in _request.FieldList)
                        {
                            data.Add(SBStringFlag.NTSStringFlag);
                            data.AddRange(Encoding.ASCII.GetBytes(server.ServerData.StandardKeyValue[key]));
                            data.Add(SBStringFlag.StringSpliter);
                        }
                        break;
                    case SBServerListUpdateOption.NoServerList:

                        break;
                    case SBServerListUpdateOption.PushUpdates:
                        break;

                    case SBServerListUpdateOption.SendGroups:
                        //TODO add values here
                        foreach (var room in _redisGroup)
                        {
                            data.Add((byte)GameServerFlags.HasKeysFlag);
                            data.AddRange(new byte[] { 192, 168, 0, 1 });
                            data.Add(SBStringFlag.NTSStringFlag);
                            data.AddRange(Encoding.ASCII.GetBytes(room.Name));
                            data.Add(SBStringFlag.StringSpliter);

                            data.Add(SBStringFlag.NTSStringFlag);
                            data.AddRange(Encoding.ASCII.GetBytes(room.Numwaiting.ToString()));
                            data.Add(SBStringFlag.StringSpliter);

                            data.Add(SBStringFlag.NTSStringFlag);
                            data.AddRange(Encoding.ASCII.GetBytes(room.Maxwaiting.ToString()));
                            data.Add(SBStringFlag.StringSpliter);

                            data.Add(SBStringFlag.NTSStringFlag);
                            data.AddRange(Encoding.ASCII.GetBytes(room.Name));
                            data.Add(SBStringFlag.StringSpliter);

                            data.Add(SBStringFlag.NTSStringFlag);
                            data.AddRange(Encoding.ASCII.GetBytes(room.Numservers.ToString()));
                            data.Add(SBStringFlag.StringSpliter);

                            data.Add(SBStringFlag.NTSStringFlag);
                            data.AddRange(Encoding.ASCII.GetBytes(room.Numplayers.ToString()));
                            data.Add(SBStringFlag.StringSpliter);
                        }
                        break;

                }
            }
            return data;
        }

        private List<byte> GenerateServerInfoHeader(DedicatedGameServer server)
        {
            // you will only have HasKeysFlag or HasFullRule you can not have both
            List<byte> data = new List<byte>();

            switch (_request.UpdateOption)
            {
                case SBServerListUpdateOption.NoServerList:
                    //we do not need to add anything here client will only read game port
                    break;


                case SBServerListUpdateOption.GeneralRequest:
                    //add has key flag
                    data.Add((byte)GameServerFlags.HasKeysFlag);
                    //we add server public ip here
                    data.AddRange(BitConverter.GetBytes(server.RemoteIP));
                    //we check host port is standard port or not
                    CheckNonStandardPort(data, server);
                    // now we check if there are private ip
                    CheckPrivateIP(data, server);
                    // we check private port here
                    CheckPrivatePort(data, server);
                    //we check icmp support here
                    CheckICMPSupport(data, server);
                    break;

                case SBServerListUpdateOption.PushUpdates:
                    break;


                case SBServerListUpdateOption.SendGroups:
                    data.Add((byte)GameServerFlags.HasKeysFlag);
                    data.AddRange(new byte[] { 0, 0, 0, 0 });
                    break;
            }

            return data;
        }

        private void CheckPrivateIP(List<byte> header,DedicatedGameServer server)
        {
            // now we check if there are private ip
            if (server.ServerData.StandardKeyValue.ContainsKey("localip0"))
            {
                header[0] ^= (byte)GameServerFlags.PrivateIPFlag;
                byte[] address = IPAddress.Parse(server.ServerData.StandardKeyValue["localip0"]).GetAddressBytes();
                header.AddRange(address);
            }
            else if (server.ServerData.StandardKeyValue.ContainsKey("localip1"))
            {
                header[0] ^= (byte)GameServerFlags.PrivateIPFlag;
                byte[] address = IPAddress.Parse(server.ServerData.StandardKeyValue["localip1"]).GetAddressBytes();
                header.AddRange(address);
            }
        }
        private void CheckNonStandardPort(List<byte> header, DedicatedGameServer server)
        {
            //we check host port is standard port or not
            if (server.ServerData.StandardKeyValue.ContainsKey("hostport"))
            {
                if (server.ServerData.StandardKeyValue["hostport"] != "6500")
                {
                    header[0] ^= (byte)GameServerFlags.NonStandardPort;
                    //we do not need to reverse port bytes
                    byte[] port = BitConverter.GetBytes(ushort.Parse(server.ServerData.StandardKeyValue["hostport"]));

                    header.AddRange(port);
                }
            }
        }
        private void CheckPrivatePort(List<byte> header, DedicatedGameServer server)
        {
            // we check private port here
            if (server.ServerData.StandardKeyValue.ContainsKey("localport"))
            {
                header[0] ^= (byte)GameServerFlags.NonStandardPrivatePortFlag;
                byte[] localPort =
                 BitConverter.GetBytes(ushort.Parse(server.ServerData.StandardKeyValue["localport"]));

                header.AddRange(localPort);
            }
        }
        private void CheckICMPSupport(List<byte> header, DedicatedGameServer server)
        {

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
                    return false;
                }
            }
        }
    }
}
