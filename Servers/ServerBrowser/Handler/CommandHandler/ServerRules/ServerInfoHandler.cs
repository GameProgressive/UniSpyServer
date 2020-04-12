using GameSpyLib.Encryption;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using GameSpyLib.MiscMethod;
using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure;
using ServerBrowser.Entity.Structure.Packet.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ServerBrowser.Handler.CommandHandler.ServerInfo
{
    /// <summary>
    /// Get full rules for a server (for example, to get
    /// player information from a server that only has basic information so far)
    /// </summary>
    public class ServerInfoHandler : CommandHandlerBase
    {
        private ServerRulesRequest _request;
        private  GameServer _gameServer;
        public ServerInfoHandler() : base()
        {
        }

        public override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);
            _request = new ServerRulesRequest();
            if (!_request.Parse(recv))
            {
                _errorCode = SBErrorCode.Parse;
                return;
            }

        }

        public override void DataOperation(SBSession session, byte[] recv)
        {
            base.DataOperation(session, recv);
            var result =
                GameServer.GetGameServers(
                    new IPAddress(BitConverter.GetBytes(_request.IP)).ToString());
            if (result.Count() != 1)
            {
                _errorCode = SBErrorCode.NoServersFound;
                return;
            }

            _gameServer = result.FirstOrDefault();
        }

        public override void ConstructResponse(SBSession session, byte[] recv)
        {
            base.ConstructResponse(session,recv);

            List<byte> data = new List<byte>();

            data.Add((byte)SBServerResponseType.PushServerMessage);

            byte[] info = GenerateServerInfo().ToArray();

            // we add server info here
            data.AddRange(info);
            LogWriter.ToLog("[Plain] "+
                StringExtensions.ReplaceUnreadableCharToHex(info));

            byte[] byteLength = BitConverter.GetBytes((short)(info.Length+2));
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(byteLength);
            }
            data.InsertRange(0, byteLength);


            GOAEncryption enc = new GOAEncryption(session.EncState);

            _sendingBuffer = enc.Encrypt(data.ToArray());
            session.EncState = enc.State;
        }

        private List<byte> GenerateServerInfo()
        {
            List<byte> data = new List<byte>();
            data.AddRange(GenerateServerInfoHeader(_gameServer));

            foreach (var kv in _gameServer.ServerData.KeyValue)
            {
                data.AddRange(Encoding.ASCII.GetBytes(kv.Key));
                data.Add(SBStringFlag.StringSpliter);
                data.AddRange(Encoding.ASCII.GetBytes(kv.Value));
                data.Add(SBStringFlag.StringSpliter);
            }
            foreach (var player in _gameServer.PlayerData.KeyValueList)
            {
                foreach (var kv in player)
                {
                    data.AddRange(Encoding.ASCII.GetBytes(kv.Key));
                    data.Add(SBStringFlag.StringSpliter);
                    data.AddRange(Encoding.ASCII.GetBytes(kv.Value));
                    data.Add(SBStringFlag.StringSpliter);
                }
            }
            foreach (var team in _gameServer.TeamData.KeyValueList)
            {
                foreach (var kv in team)
                {
                    data.AddRange(Encoding.ASCII.GetBytes(kv.Key));
                    data.Add(SBStringFlag.StringSpliter);
                    data.AddRange(Encoding.ASCII.GetBytes(kv.Value));
                    data.Add(SBStringFlag.StringSpliter);
                }
            }

            return data;
        }
        private List<byte> GenerateServerInfoHeader(GameServer server)
        {
            // you will only have HasKeysFlag or HasFullRule you can not have both
            List<byte> header = new List<byte>();

            //add has key flag
            header.Add((byte)GameServerFlags.HasFullRulesFlag);

            //we add server public ip here
            header.AddRange(BitConverter.GetBytes(server.RemoteIP));

            //we check host port is standard port or not
            CheckNonStandardPort(header, server);

            // now we check if there are private ip
            CheckPrivateIP(header, server);

            // we check private port here
            CheckPrivatePort(header, server);

            //TODO we have to check icmp_ip_flag

            return header;
        }
        private void CheckPrivateIP(List<byte> header,  GameServer server)
        {
            // now we check if there are private ip
            if (server.ServerData.KeyValue.ContainsKey("localip0"))
            {
                header[0] ^= (byte)GameServerFlags.PrivateIPFlag;
                byte[] address = IPAddress.Parse(server.ServerData.KeyValue["localip0"]).GetAddressBytes();
                header.AddRange(address);
            }
            else if (server.ServerData.KeyValue.ContainsKey("localip1"))
            {
                header[0] ^= (byte)GameServerFlags.PrivateIPFlag;
                byte[] address = IPAddress.Parse(server.ServerData.KeyValue["localip1"]).GetAddressBytes();
                header.AddRange(address);
            }
        }
        private void CheckNonStandardPort(List<byte> header,  GameServer server)
        {
            //we check host port is standard port or not
            if (server.ServerData.KeyValue.ContainsKey("hostport"))
            {
                if (server.ServerData.KeyValue["hostport"] != "6500")
                {
                    header[0] ^= (byte)GameServerFlags.NonStandardPort;
                    //we do not need to reverse port bytes
                    byte[] port = BitConverter.GetBytes(ushort.Parse(server.ServerData.KeyValue["hostport"]));

                    header.AddRange(port);
                }
            }
        }
        private void CheckPrivatePort(List<byte> header,  GameServer server)
        {
            // we check private port here
            if (server.ServerData.KeyValue.ContainsKey("localport"))
            {
                header[0] ^= (byte)GameServerFlags.NonStandardPrivatePortFlag;
                byte[] localPort =
                 BitConverter.GetBytes(ushort.Parse(server.ServerData.KeyValue["localport"]));

                header.AddRange(localPort);
            }
        }

    }
}
