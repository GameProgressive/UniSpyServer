
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.GameServer;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.PeerGroup;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.ServerBrowser.Application
{
    public class StorageOperation : ServerBrowser.Abstraction.Interface.IStorageOperation
    {
        private QueryReport.Entity.Structure.Redis.GameServer.RedisClient _gameServerRedisClient = new QueryReport.Entity.Structure.Redis.GameServer.RedisClient();
        private QueryReport.Entity.Structure.Redis.PeerGroup.RedisClient _peerGroupRedisClient = new QueryReport.Entity.Structure.Redis.PeerGroup.RedisClient();
        public static ServerBrowser.Abstraction.Interface.IStorageOperation Persistance = new StorageOperation();

        public GameServerInfo GetGameServerInfo(IPAddress address, ushort queryReportPort)
        {
            return _gameServerRedisClient.Context.FirstOrDefault(x =>
                x.HostIPAddress == address &
                x.QueryReportPort == queryReportPort);
        }

        public List<GameServerInfo> GetGameServerInfos(string gameName)
        {
            return _gameServerRedisClient.Context.Where(x => x.GameName == gameName).ToList();
        }

        public PeerGroupInfo GetPeerGroupInfo(string gameName)
        {
            return _peerGroupRedisClient.Context.FirstOrDefault(x => x.GameName == gameName);
        }

        public IQueryable<Grouplist> GetGroupList(string gameName)
        {
            using (var db = new UniSpyContext())
            {
                var result = from g in db.Games
                             join gl in db.Grouplists on g.Gameid equals gl.Gameid
                             where g.Gamename == gameName
                             select gl;
                return result;
            }

        }
    }
}