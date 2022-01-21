using System.Linq;
using UniSpyServer.Servers.ServerBrowser.Entity.Contract;
using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;
using UniSpyServer.Servers.ServerBrowser.Entity.Enumerate;
using UniSpyServer.Servers.ServerBrowser.Entity.Exception;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Packet.Response;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Response.ServerList;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
namespace UniSpyServer.Servers.ServerBrowser.Handler.CmdHandler
{
    [HandlerContract(RequestType.ServerListRequest)]
    public class ServerListHandler : ServerListUpdateOptionHandlerBase
    {
        public ServerListHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void DataOperation()
        {
            //todo check protocol version!!!!

            switch (_request.UpdateOption)
            {
                case ServerListUpdateOption.ServerMainList:
                    ServerMainList();
                    break;
                case ServerListUpdateOption.P2PServerMainList:
                case ServerListUpdateOption.LimitResultCount:
                    P2PServerMainList();
                    break;
                case ServerListUpdateOption.P2PGroupRoomList:
                    P2PGroupRoomList();
                    break;
                case ServerListUpdateOption.ServerNetworkInfoList:
                    ServerNetworkInfoList();
                    break;
                default:
                    throw new SBException("unknown serverlist update option type");
            }
        }
        private void P2PGroupRoomList()
        {
            _result = new P2PGroupRoomListResult();
            var peerGroupInfo = _peerGroupRedisClient.Values.Where(x => x.GameName == _request.GameName).FirstOrDefault();
            // Game name is unique in redis database
            if (peerGroupInfo == null)
            {
                throw new SBException("can not find peer group info in redis database");
            }
            ((P2PGroupRoomListResult)_result).PeerGroupInfo = peerGroupInfo;
        }
        private void P2PServerMainList()
        {
            _result = new ServerMainListResult();
            var gameServerInfos = _gameServerRedisClient.Values.Where(x => x.GameName == _request.GameName).ToList();
            ((ServerMainListResult)_result).GameServerInfos = gameServerInfos;
            ((ServerMainListResult)_result).Flag = GameServerFlags.HasFullRulesFlag;
            //TODO do filter
            //**************Currently we do not handle filter**********************
        }
        private void ServerMainList()
        {
            _result = new ServerMainListResult();
            var gameServerInfos = _gameServerRedisClient.Values.Where(x => x.GameName == _request.GameName).ToList();
            ((ServerMainListResult)_result).GameServerInfos = gameServerInfos;
            ((ServerMainListResult)_result).Flag = GameServerFlags.HasKeysFlag;
        }
        private void ServerNetworkInfoList()
        {
            _result = new ServerMainListResult();
        }


        protected override void ResponseConstruct()
        {
            switch (_request.UpdateOption)
            {
                case ServerListUpdateOption.ServerMainList:
                case ServerListUpdateOption.P2PServerMainList:
                case ServerListUpdateOption.LimitResultCount:
                    _response = new ServerMainListResponse(_request, _result);
                    break;
                case ServerListUpdateOption.P2PGroupRoomList:
                    _response = new P2PGroupRoomListResponse(_request, _result);
                    break;
                case ServerListUpdateOption.ServerNetworkInfoList:
                    _response = new ServerNetworkInfoListResponse(_request, _result);
                    break;
                default:
                    throw new SBException("unknown serverlist update option type");
            }
        }
    }
}