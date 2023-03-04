
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UniSpy.Server.QueryReport.Aggregate.Redis.GameServer;
using UniSpy.Server.QueryReport.Aggregate.Redis.PeerGroup;
using UniSpy.Server.QueryReport.Contract.Request;
using UniSpy.Server.Core.Database.DatabaseModel;

namespace UniSpy.Server.ServerBrowser.Application
{
    internal sealed class StorageOperation : ServerBrowser.Abstraction.Interface.IStorageOperation
    {
        public static ServerBrowser.Abstraction.Interface.IStorageOperation Persistance = new StorageOperation();
        private static QueryReport.Aggregate.Redis.GameServer.RedisClient _gameServerRedisClient = new QueryReport.Aggregate.Redis.GameServer.RedisClient();
        private static QueryReport.Aggregate.Redis.PeerGroup.RedisClient _peerGroupRedisClient = new QueryReport.Aggregate.Redis.PeerGroup.RedisClient();
        /// <summary>
        /// The redis channel use to transfer client message
        /// </summary>
        /// <returns></returns>
        public static QueryReport.Aggregate.Redis.RedisChannel Channel = new QueryReport.Aggregate.Redis.RedisChannel();

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
        public Dictionary<string, List<Grouplist>> GetAllGroupList()
        {
            using (var db = new UniSpyContext())
            {
                var result = from g in db.Games
                             join gl in db.Grouplists on g.Gameid equals gl.Gameid
                             group gl by g.Gamename into dd
                             select new KeyValuePair<string, List<Grouplist>>(dd.Key, dd.ToList());

                var data = result.ToDictionary(x => x.Key, x => x.Value);
                return data;
            }
        }
        public void PublishClientMessage(ClientMessageRequest message)
        {
            Channel.PublishMessage(message);
        }

        public GameServerInfo GetGameServerInfo(IPEndPoint end)
        {
            return _gameServerRedisClient.Context.FirstOrDefault(x =>
                x.HostIPAddress == end.Address &
                x.QueryReportPort == (ushort)end.Port);
        }
    }
}