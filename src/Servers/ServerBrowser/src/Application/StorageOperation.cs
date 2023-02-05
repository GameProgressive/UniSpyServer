
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Redis.GameServer;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Redis.PeerGroup;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.ServerBrowser.Application
{
    internal sealed class StorageOperation : ServerBrowser.Abstraction.Interface.IStorageOperation
    {
        public static ServerBrowser.Abstraction.Interface.IStorageOperation Persistance = new StorageOperation();
        private static QueryReport.V2.Entity.Structure.Redis.GameServer.RedisClient _gameServerRedisClient = new QueryReport.V2.Entity.Structure.Redis.GameServer.RedisClient();
        private static QueryReport.V2.Entity.Structure.Redis.PeerGroup.RedisClient _peerGroupRedisClient = new QueryReport.V2.Entity.Structure.Redis.PeerGroup.RedisClient();
        /// <summary>
        /// The redis channel use to transfer client message
        /// </summary>
        /// <returns></returns>
        private static QueryReport.V2.Entity.Structure.Redis.RedisChannel _clientMessageRedisChannel = new QueryReport.V2.Entity.Structure.Redis.RedisChannel();

        public List<GameServerInfo> GetGameServerInfos(string gameName)
        {
            return _gameServerRedisClient.Context.Where(x => x.GameName == gameName).ToList();
        }

        public PeerGroupInfo GetPeerGroupInfo(string gameName)
        {
            return _peerGroupRedisClient.Context.FirstOrDefault(x => x.GameName == gameName);
        }

        public List<Grouplist> GetGroupList(string gameName)
        {
            using (var db = new UniSpyContext())
            {
                var result = from g in db.Games
                             join gl in db.Grouplists on g.Gameid equals gl.Gameid
                             where g.Gamename == gameName
                             select gl;
                return result.ToList();
            }

        }
        public void PublishClientMessage(ClientMessageRequest message)
        {
            _clientMessageRedisChannel.PublishMessage(message);
        }

        public GameServerInfo GetGameServerInfo(IPEndPoint end)
        {
            return _gameServerRedisClient.Context.FirstOrDefault(x =>
                x.HostIPAddress == end.Address &
                x.QueryReportPort == (ushort)end.Port);
        }
    }
}