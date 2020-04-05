using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.Encryption;
using GameSpyLib.Extensions;
using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure.Packet.Request;
using ServerBrowser.Entity.Structure.Packet.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler
{
    public abstract class UpdateOptionHandlerBase : CommandHandlerBase
    {
        protected byte[] _clientRemoteIP;
        protected byte[] _gameServerDefaultHostPort;
        protected string _secretKey;
        protected ServerListRequest _request;
        protected List<byte> _dataList = new List<byte>();
        public UpdateOptionHandlerBase(ServerListRequest request) : base()
        {
            _request = request;
        }

        public override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);
            //save client challenge in _request
            if (_request == null)
            {
                _errorCode = SBErrorCode.Parse;
                return;
            }
            //we first check and get secrete key from database
            if (!DataOperationExtensions.GetSecretKey(_request.GameName, out _secretKey))
            {
                _errorCode = SBErrorCode.UnSupportedGame;
                return;
            }
            //this is client public ip and default query port
            _clientRemoteIP = ((IPEndPoint)session.Socket.RemoteEndPoint).Address.GetAddressBytes();
            //TODO   //check what is the default port
            _gameServerDefaultHostPort = BitConverter.GetBytes((ushort)(6500 & 0xFFFF));
        }

        protected virtual void GenerateServerKeys() { }
        protected virtual void GenerateUniqueValue() { }
        protected virtual void GenerateServersInfo() { }
        protected virtual void GenerateServerInfoHeader(List<byte> header, DedicatedGameServer server) { }




        protected void CheckPrivateIP(List<byte> header, DedicatedGameServer server)
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
        protected void CheckNonStandardPort(List<byte> header, DedicatedGameServer server)
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
        protected void CheckPrivatePort(List<byte> header, DedicatedGameServer server)
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
        protected void CheckICMPSupport(List<byte> header, DedicatedGameServer server)
        {

        }

        /// <summary>
        /// Add ip and port
        /// </summary>
        /// <param name="session"></param>
        /// <param name="recv"></param>
        public override void ConstructResponse(SBSession session, byte[] recv)
        {
            base.ConstructResponse(session, recv);
            _dataList.AddRange(_clientRemoteIP);
            _dataList.AddRange(_gameServerDefaultHostPort);

        }
        public override void Response(SBSession session, byte[] recv)
        {
            session.LogPlainText(Encoding.ASCII.GetString(_dataList.ToArray()));

            GOAEncryption enc =
                new GOAEncryption(_secretKey, _request.Challenge, SBServer.ServerChallenge);

            _sendingBuffer = new ServerListResponse().
                CombineHeaderAndContext
                (
                    enc.Encrypt(_dataList.ToArray()),
                     SBServer.ServerChallenge
                );
            //refresh encryption state
            session.EncState = enc.State;

            base.Response(session, recv);
        }
    }
}
