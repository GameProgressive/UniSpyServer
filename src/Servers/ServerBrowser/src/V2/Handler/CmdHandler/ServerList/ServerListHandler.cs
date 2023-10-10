using System.Linq;
using UniSpy.Server.ServerBrowser.V2.Application;
using UniSpy.Server.ServerBrowser.V2.Abstraction.BaseClass;
using UniSpy.Server.ServerBrowser.V2.Enumerate;
using UniSpy.Server.ServerBrowser.V2.Aggregate.Packet.Response;
using UniSpy.Server.ServerBrowser.V2.Contract.Response.ServerList;
using UniSpy.Server.ServerBrowser.V2.Contract.Result;
using UniSpy.Server.ServerBrowser.V2.Contract.Request;
using UniSpy.Server.QueryReport.Aggregate.Redis.PeerGroup;
using System.Collections.Generic;

namespace UniSpy.Server.ServerBrowser.V2.Handler.CmdHandler
{

    public class ServerListHandler : ServerListUpdateOptionHandlerBase
    {
        public ServerListHandler(Client client, ServerListRequest request) : base(client, request)
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

            // first get the peer room in memory, if there is no such game we do not continue
            if (!Chat.Application.StorageOperation.Persistance.PeerGroupList.ContainsKey(_request.GameName))
            {
                throw new ServerBrowser.Exception($"Invalid game name: {_request.GameName}.");
            }

            // Game name is unique in redis database
            var grouplist = Chat.Application.StorageOperation.Persistance.PeerGroupList[_request.GameName];
            // we do not create peer room cache on redis, we just send peer room info to client
            var tempInfos = new List<PeerRoomInfo>();
            foreach (var group in grouplist)
            {
                // we create room info, set the room properties to default
                var roomInfo = new PeerRoomInfo(group.Game.Gamename, group.Groupid, group.Roomname);
                tempInfos.Add(roomInfo);
                // get the channels info from redis where groupid equals above
                var groupRooms = QueryReport.Application.StorageOperation.Persistance.GetPeerGroupChannel(group.Groupid);
                // get the channels info from redis where created under gamename and groupid above
                var stagingRooms = QueryReport.Application.StorageOperation.Persistance.GetPeerStagingChannel(group.Game.Gamename, group.Groupid);
                if (groupRooms.Count != 0)
                {
                    roomInfo.NumberOfWaitingPlayers = groupRooms.Sum(r => r.Users.Count);
                }
                if (stagingRooms.Count != 0)
                {
                    roomInfo.NumberOfPlayingPlayers = stagingRooms.Sum(r => r.Users.Count);
                    roomInfo.NumberOfGames = stagingRooms.Count;
                }
                roomInfo.NumberOfPlayers = roomInfo.NumberOfWaitingPlayers + roomInfo.NumberOfPlayingPlayers;
                // if there did not have any rooms in redis, the properties in roominfo stay default
            }
            ((P2PGroupRoomListResult)_result).PeerRoomsInfo = tempInfos;
        }
        private void P2PServerMainList()
        {
            var serverInfos = QueryReport.V2.Application.StorageOperation.Persistance.GetGameServerInfos(_request.GameName);
            ((ServerMainListResult)_result).Flag = GameServerFlags.HasKeysFlag;
            var filteredGameServerInfos = new List<QueryReport.V2.Aggregate.Redis.GameServer.GameServerCache>();
            //TODO do filter
            if (_request.Filter is not null)
            {
                if (_request.Filter.Contains("groupid="))
                {
                    var groupId = _request.Filter.Replace("groupid=", "");
                    filteredGameServerInfos = serverInfos.Where(s => s.ServerData.ContainsKey("groupid=")).Where(s => s.ServerData["groupid"] == groupId).ToList();
                    ((ServerMainListResult)_result).GameServerInfos = filteredGameServerInfos;
                }
                else
                {
                    filteredGameServerInfos = serverInfos;
                }
            }
            else
            {
                filteredGameServerInfos = serverInfos;
            }

            ((ServerMainListResult)_result).GameServerInfos = filteredGameServerInfos;
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