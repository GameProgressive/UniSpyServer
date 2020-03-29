using System;
using System.Collections.Generic;
using System.Text;
using GameSpyLib.Extensions;
using QueryReport.Entity.Structure;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure;

namespace ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler.GeneralRequest
{
    public class GeneralRequestHandler:UpdateOptionHandlerBase
    {
        List<DedicatedGameServer> _gameServer;
        public GeneralRequestHandler(SBSession session, byte[] recv) : base(session, recv)
        {
        }

      

        public override void DataOperation(SBSession session, byte[] recv)
        {
            base.DataOperation(session, recv);
            _gameServer =
                        RetroSpyRedisExtensions.GetDedicatedGameServers<DedicatedGameServer>(_request.GameName);
            if(_gameServer==null||_gameServer.Count==0)
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
            //add key number
            _dataList.Add((byte)_request.FieldList.Length);
            foreach (var key in _request.FieldList)
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
                GenerateServerInfoHeader(server);
                foreach (var key in _request.FieldList)
                {
                    _dataList.Add(SBStringFlag.NTSStringFlag);
                    _dataList.AddRange(Encoding.ASCII.GetBytes(server.ServerData.StandardKeyValue[key]));
                    _dataList.Add(SBStringFlag.StringSpliter);
                }
            }
        }


        protected override void GenerateServerInfoHeader(DedicatedGameServer server)
        {
            //add has key flag
            _dataList.Add((byte)GameServerFlags.HasKeysFlag);
            //we add server public ip here
            _dataList.AddRange(BitConverter.GetBytes(server.RemoteIP));
            //we check host port is standard port or not
            CheckNonStandardPort(_dataList, server);
            // now we check if there are private ip
            CheckPrivateIP(_dataList, server);
            // we check private port here
            CheckPrivatePort(_dataList, server);
            //we check icmp support here
            CheckICMPSupport(_dataList, server);
        }
    }
}
