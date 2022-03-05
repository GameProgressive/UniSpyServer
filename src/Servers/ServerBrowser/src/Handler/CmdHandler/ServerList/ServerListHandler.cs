using System.Linq;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.PeerGroup;
using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;
using UniSpyServer.Servers.ServerBrowser.Entity.Contract;
using UniSpyServer.Servers.ServerBrowser.Entity.Enumerate;
using UniSpyServer.Servers.ServerBrowser.Entity.Exception;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Packet.Response;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Response.ServerList;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.ServerBrowser.Handler.CmdHandler
{
    [HandlerContract(RequestType.ServerListRequest)]
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
            _result.ClientRemoteIP = _client.Session.RemoteIPEndPoint.Address.GetAddressBytes();
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
            var groupInfo = _peerGroupRedisClient.Values.FirstOrDefault(x => x.GameName == _request.GameName);
            if (groupInfo == null)
            {
                // search gamename in database
                using (var db = new UniSpyContext())
                {
                    var result = from g in db.Games
                                 join gl in db.Grouplists on g.Gameid equals gl.Gameid
                                 where g.Gamename == _request.GameName
                                 select gl;
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
            }
            ((P2PGroupRoomListResult)_result).PeerGroupInfo = groupInfo;
        }
        private void P2PServerMainList()
        {
            var serverInfos = _gameServerRedisClient.Values.Where(x => x.GameName == _request.GameName).ToList();
            ((ServerMainListResult)_result).GameServerInfos = serverInfos;
            ((ServerMainListResult)_result).Flag = GameServerFlags.HasKeysFlag;
            //TODO do filter
            //**************Currently we do not handle filter**********************
        }
        private void ServerMainList()
        {
            var serverInfos = _gameServerRedisClient.Values.Where(x => x.GameName == _request.GameName).ToList();
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