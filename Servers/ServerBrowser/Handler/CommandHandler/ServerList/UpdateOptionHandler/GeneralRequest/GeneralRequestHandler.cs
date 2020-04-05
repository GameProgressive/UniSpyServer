using GameSpyLib.Extensions;
using GameSpyLib.MiscMethod;
using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure;
using ServerBrowser.Entity.Structure.Packet.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.GeneralRequest
{
    public class GeneralRequestHandler : UpdateOptionHandlerBase
    {
        List<DedicatedGameServer> _gameServer;
        public GeneralRequestHandler(ServerListRequest request) : base(request)
        {
        }

        public override void DataOperation(SBSession session, byte[] recv)
        {
            base.DataOperation(session, recv);
            _gameServer =
                        RedisExtensions.GetDedicatedGameServers<DedicatedGameServer>(_request.GameName);
            if (_gameServer == null || _gameServer.Count == 0)
            {
                _errorCode = SBErrorCode.NoServersFound;
                return;
            }
        }

        public override void ConstructResponse(SBSession session, byte[] recv)
        {

            base.ConstructResponse(session, recv);

            GenerateServerKeys();
            //we use NTS string so total unique value list is 0
            GenerateUniqueValue();
            //add server infomation such as public ip etc.
            GenerateServersInfo();
            //after all server information is added we add the end flag
            _dataList.AddRange(SBStringFlag.AllServerEndFlag);
        }

        protected override void GenerateServerKeys()
        {
            //we add the total number of the requested keys
            _dataList.Add((byte)_request.Keys.Length);
            //then we add the keys
            foreach (var key in _request.Keys)
            {
                _dataList.Add((byte)SBKeyType.String);
                _dataList.AddRange(Encoding.ASCII.GetBytes(key));
                _dataList.Add(0);
            }
        }

        protected override void GenerateUniqueValue()
        {
            //because we are using NTS string so we do not have any value here
            _dataList.Add(0);
        }

        protected override void GenerateServersInfo()
        {

            foreach (var server in _gameServer)
            {
                List<byte> header = new List<byte>();
                GenerateServerInfoHeader(header, server);
                _dataList.AddRange(header);
                foreach (var key in _request.Keys)
                {
                    _dataList.Add(SBStringFlag.NTSStringFlag);
                    _dataList.AddRange(Encoding.ASCII.GetBytes(server.ServerData.StandardKeyValue[key]));
                    _dataList.Add(SBStringFlag.StringSpliter);
                }
            }
        }


        protected override void GenerateServerInfoHeader(List<byte> header, DedicatedGameServer server)
        {
            //add has key flag
            header.Add((byte)GameServerFlags.HasKeysFlag);
            //we add server public ip here
            header.AddRange(BitConverter.GetBytes(server.RemoteIP));
            //we check host port is standard port or not
            CheckNonStandardPort(header, server);
            // now we check if there are private ip
            CheckPrivateIP(header, server);
            // we check private port here
            CheckPrivatePort(header, server);
            //we check icmp support here
            CheckICMPSupport(header, server);
        }
    }
}
