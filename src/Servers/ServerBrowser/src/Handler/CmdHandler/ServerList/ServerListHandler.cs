using System.Linq;
using ServerBrowser.Entity.Contract;
using QueryReport.Entity.Structure.Redis;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Exception;
using ServerBrowser.Entity.Structure.Packet.Response;
using ServerBrowser.Entity.Structure.Response.ServerList;
using ServerBrowser.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;
namespace ServerBrowser.Handler.CmdHandler
{
    [HandlerContract(RequestType.ServerListRequest)]
    internal class ServerListHandler : ServerListUpdateOptionHandlerBase
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
            var searchKey = new PeerGroupInfoRedisKey()
            {
                GameName = _request.GameName
            };
            var matchedKey = PeerGroupInfoRedisOperator.GetMatchedKeys(searchKey);

            // Game name is unique in redis database
            if (matchedKey.Count != 1)
            {
                throw new SBException("There are no group room found in database.");
            }
            ((P2PGroupRoomListResult)_result).PeerGroupInfo = PeerGroupInfoRedisOperator.GetSpecificValue(matchedKey[0]);
        }
        private void P2PServerMainList()
        {
            _result = new ServerMainListResult();
            var searchKey = new GameServerInfoRedisKey()
            {
                GameName = _request.GameName
            };

            ((ServerMainListResult)_result).GameServerInfos = GameServerInfoRedisOperator.GetMatchedKeyValues(searchKey).Values.ToList();
            ((ServerMainListResult)_result).Flag = GameServerFlags.HasFullRulesFlag;
            //TODO do filter
            //**************Currently we do not handle filter**********************
        }
        private void ServerMainList()
        {
            _result = new ServerMainListResult();

            var searchKey = new GameServerInfoRedisKey()
            {
                GameName = _request.GameName
            };

            ((ServerMainListResult)_result).GameServerInfos = GameServerInfoRedisOperator.GetMatchedKeyValues(searchKey).Values.ToList();
            ((ServerMainListResult)_result).Flag = GameServerFlags.HasKeysFlag;
        }
        private void ServerNetworkInfoList() { _result = new ServerMainListResult(); }


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