using System.Linq;
using UniSpy.Server.QueryReport.V2.Aggregate.Redis.PeerGroup;
using UniSpy.Server.ServerBrowser.V2.Application;
using UniSpy.Server.ServerBrowser.V2.Abstraction.BaseClass;
using UniSpy.Server.ServerBrowser.V2.Enumerate;

using UniSpy.Server.ServerBrowser.V2.Aggregate.Packet.Response;
using UniSpy.Server.ServerBrowser.V2.Contract.Response.ServerList;
using UniSpy.Server.ServerBrowser.V2.Contract.Result;
using UniSpy.Server.Core.Abstraction.Interface;
using System.Collections.Generic;
using UniSpy.Server.QueryReport.V2.Aggregate.Redis.GameServer;

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
                    throw new ServerBrowser.Exception("unknown serverlist update option type");
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
                    throw new ServerBrowser.Exception("unknown serverlist update option type");
            }
            _client.Info.SearchType = _request.UpdateOption;

        }
        private void P2PGroupRoomList()
        {
            // we first get the peer rooms in redis
            // Game name is unique in redis database
            var peerRooms = QueryReport.V2.Application.StorageOperation.Persistance.GetPeerRoomsInfo(_request.GameName);
            // get the peer room in database
            var grouplist = QueryReport.V2.Application.StorageOperation.Persistance.GetGroupList(_request.GameName);
            // check if there are missing peer rooms in redis
            var addInfos = grouplist.Where(g => peerRooms.All(r => r.GroupId != g.Groupid)).ToList();
            foreach (var room in addInfos)
            {
                var roomInfo = new PeerRoomInfo(room.Game.Gamename, room.Groupid, room.Roomname);
                QueryReport.V2.Application.StorageOperation.Persistance.UpdatePeerRoomInfo(roomInfo);
            }
            ((P2PGroupRoomListResult)_result).PeerRoomsInfo = QueryReport.V2.Application.StorageOperation.Persistance.GetPeerRoomsInfo(_request.GameName);
        }
        private void P2PServerMainList()
        {
            var serverInfos = QueryReport.V2.Application.StorageOperation.Persistance.GetGameServerInfos(_request.GameName);
            ((ServerMainListResult)_result).Flag = GameServerFlags.HasKeysFlag;

            //TODO do filter
            if (_request.Filter.Contains("groupid="))
            {
                var groupId = _request.Filter.Replace("groupid=", "");
                var filteredGameServerInfos = serverInfos.Where(s => s.ServerData.ContainsKey("groupid=")).Where(s => s.ServerData["groupid"] == groupId).ToList();
                ((ServerMainListResult)_result).GameServerInfos = filteredGameServerInfos;
            }
            else
            {
                ((ServerMainListResult)_result).GameServerInfos = serverInfos;
            }

        }
        private void ServerMainList()
        {
            var serverInfos = QueryReport.V2.Application.StorageOperation.Persistance.GetGameServerInfos(_request.GameName);
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
                    throw new ServerBrowser.Exception("unknown serverlist update option type");
            }
        }
    }
}