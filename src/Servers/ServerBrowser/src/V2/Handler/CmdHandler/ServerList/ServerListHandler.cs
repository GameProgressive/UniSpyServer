using System.Linq;
using UniSpy.Server.QueryReport.V2.Entity.Structure.Redis.PeerGroup;
using UniSpy.Server.ServerBrowser.Application;
using UniSpy.Server.ServerBrowser.V2.Abstraction.BaseClass;
using UniSpy.Server.ServerBrowser.V2.Entity.Enumerate;
using UniSpy.Server.ServerBrowser.V2.Entity.Exception;
using UniSpy.Server.ServerBrowser.V2.Entity.Structure.Packet.Response;
using UniSpy.Server.ServerBrowser.V2.Entity.Structure.Response.ServerList;
using UniSpy.Server.ServerBrowser.V2.Entity.Structure.Result;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.ServerBrowser.V2.Handler.CmdHandler
{

    public class ServerListHandler : ServerListUpdateOptionHandlerBase
    {
        public ServerListHandler(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void RequestCheck()
        {
            base.RequestCheck();
            switch (_request.UpdateOption)
            {
                case ServerListUpdateOption.ServerMainList:
                case ServerListUpdateOption.P2PServerMainList:
                case ServerListUpdateOption.LimitResultCount:
                case ServerListUpdateOption.ServerFullInfoList:
                    _result = new ServerMainListResult();
                    break;
                case ServerListUpdateOption.P2PGroupRoomList:
                    _result = new P2PGroupRoomListResult();
                    break;
                default:
                    throw new SBException("unknown serverlist update option type");
            }
        }
        protected override void DataOperation()
        {
            _result.GameSecretKey = _client.Info.GameSecretKey;
            _result.ClientRemoteIP = _client.Connection.RemoteIPEndPoint.Address.GetAddressBytes();
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
                case ServerListUpdateOption.ServerFullInfoList:
                    // do nothing here
                    break;
                default:
                    throw new SBException("unknown serverlist update option type");
            }
        }
        private void P2PGroupRoomList()
        {
            // Game name is unique in redis database
            var groupInfo = StorageOperation.Persistance.GetPeerGroupInfo(_request.GameName);
            if (groupInfo is null)
            {
                // search gamename in database
                var result = StorageOperation.Persistance.GetGroupList(_request.GameName);
                if (result.Count() == 0)
                {
                    throw new SBException($"can not find peer group info in redis and database, please check game name:{_request.GameName}");
                }
                else
                {
                    groupInfo = new PeerGroupInfo()
                    {
                        GameName = _request.GameName,
                        GameID = result.First().Gameid,
                        PeerRooms = result.Select(x => new PeerRoomInfo(x)).ToList()
                    };
                }
            }
        ((P2PGroupRoomListResult)_result).PeerGroupInfo = groupInfo;
        }
        private void P2PServerMainList()
        {
            var serverInfos = StorageOperation.Persistance.GetGameServerInfos(_request.GameName);
            ((ServerMainListResult)_result).GameServerInfos = serverInfos;
            ((ServerMainListResult)_result).Flag = GameServerFlags.HasKeysFlag;
            //TODO do filter
            //**************Currently we do not handle filter**********************
        }
        private void ServerMainList()
        {
            var serverInfos = StorageOperation.Persistance.GetGameServerInfos(_request.GameName);
            ((ServerMainListResult)_result).GameServerInfos = serverInfos;
            ((ServerMainListResult)_result).Flag = GameServerFlags.HasKeysFlag;
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
                case ServerListUpdateOption.ServerFullInfoList:
                    _response = new ServerNetworkInfoListResponse(_request, _result);
                    break;
                default:
                    throw new SBException("unknown serverlist update option type");
            }
        }
    }
}